using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
using ProxyProject_Backend.Models.Response;
using ProxyProject_Backend.Services.Interface;
using ProxyProject_Backend.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthenticateController(
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IUserService userService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if(user.TwoFactorEnabled)
                {
                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    var emailResult = await _emailService.SendMailAsync(_configuration["EmailConfig:MFASubject"], 
                        $"<p>Your login verify code: {token}</p>", user.Email);

                    if (emailResult)
                    {
                        return Ok(new ResponseModel 
                        { 
                            Status = "Success", 
                            Message = "Send MFA email successfully!" 
                        });
                    } 
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel 
                        { 
                            Status = "Error", 
                            Message = "Send MFA email failed.Please check user details and try again." 
                        });
                    }
                }
                else
                {
                    return Ok(new ResponseModel
                    {
                        Status = "Success",
                        Data = await AuthenticateUserAsync(user)
                    });
                }
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("VerifyMFA")]
        public async Task<IActionResult> VerifyMFA([FromBody] VerifyMFAModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.VerifyTwoFactorTokenAsync(user, Constants.MFAProvider, model.Code))
            {
                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = await AuthenticateUserAsync(user)
                });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = $"{_configuration["EmailConfig:ForgotPasswordLink"]}?username={user.UserName}&code={token}";
                var emailResult = await _emailService.SendMailAsync(_configuration["EmailConfig:ForgotPasswordSubject"],
                        $"<p>Please follow this link to reset your password: {forgotPasswordLink}</p>", user.Email);

                if (emailResult)
                {
                    return Ok(new ResponseModel 
                    { 
                        Status = "Success", 
                        Message = "Send reset password email successfully!" 
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel 
                        { 
                            Status = "Error", 
                            Message = "Send reset password email failed.Please check user details and try again." 
                        });
                }
            }

            return BadRequest("User not found");
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

                if(result.Succeeded)
                {
                    return Ok(new ResponseModel 
                    { 
                        Status = "Success", 
                        Message = "Your password has been reset successfully!" 
                    });
                }

                return BadRequest("Error when reset password");
            }

            return BadRequest("User not found");
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel 
                {
                    Status = "Error", 
                    Message = "User already exists!" 
                });
            }

            UserEntity user = new UserEntity()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                APIKey = await _userService.GenerateUserAPIKeyAsync(),
                WalletKey = await _userService.GenerateUserWalletKeyAsync(),
                TwoFactorEnabled = true,
                LimitKeysToCreate = int.Parse(_configuration["LimitKeysToCreate"])
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel 
                { 
                    Status = "Error", 
                    Message = "User creation failed! Please check user details and try again.", Data = result.Errors 
                });
            }

            if (await _roleManager.RoleExistsAsync(UserRolesConstant.User))
            {
                await _userManager.AddToRoleAsync(user, UserRolesConstant.User);
            }

            return Ok(new ResponseModel 
            { 
                Status = "Success", 
                Message = "User created successfully!" 
            });
        }

        #region Private functions
        private async Task<object> AuthenticateUserAsync(UserEntity user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:Expires"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }
        #endregion
    }
}
