using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
using ProxyProject_Backend.Models.Response;
using ProxyProject_Backend.Services.Interface;
using System.Linq.Expressions;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ApiBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;

        public UserController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IUserService userService
            ) : base(context, userManager)
        {
            _configuration = configuration;
            _roleManager = roleManager;
            _userService = userService;
        }

        [HttpGet]
        [Route("GetMe")]
        public async Task<IActionResult> GetMe()
        {
            var user = await GetCurrentUser();

            if (user != null)
            {
                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new UserInfoModel
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        APIKey = user.APIKey,
                        WalletKey = user.WalletKey,
                        Balance = user.Balance,
                        TotalDeposited = user.TotalDeposited,
                        LimitKeysToCreate = user.LimitKeysToCreate,
                        NoOfCreatedKeys = await _unitOfWork.ProxyKeysRepository.CountByFilterAsync(x => x.UserId == user.Id)
                    }
                });
            }

            return BadRequest("Empty User");
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var user = await GetCurrentUser();

            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                {

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                    if (result != null)
                    {
                        return Ok(new ResponseModel
                        {
                            Status = "Success",
                            Message = "Update password successfully!"
                        });
                    }

                    return Ok(new ResponseModel
                    {
                        Status = "Error",
                        Message = "Update password failed!"
                    });
                }

                return Ok(new ResponseModel
                {
                    Status = "Error",
                    Message = "Current password is incorrect"
                });
            }

            return BadRequest("Empty User");
        }

        [HttpGet]
        [Route("ReGenerateAPIKey")]
        public async Task<IActionResult> ReGenerateAPIKey()
        {
            var user = await GetCurrentUser();

            if (user != null)
            {
                user.APIKey = await _userService.GenerateUserAPIKeyAsync();

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = user.APIKey
                });
            }

            return BadRequest("Empty User");
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUsers([FromQuery] GetListPagingModel model)
        {
            Expression<Func<UserEntity, bool>> filter = null;

            if (!string.IsNullOrWhiteSpace(model.Keyword))
            {
                filter = (x) => x.Email == model.Keyword || x.UserName == model.Keyword;
            }

            var users = await _unitOfWork.UserRepository
                     .GetAsync(filter, x => x.OrderBy(p => p.UserName), "", model.PageIndex, model.PageSize);

            return Ok(new ResponseModel
            {
                Status = "Success",
                Data = users.Select(async user => new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    APIKey = user.APIKey,
                    WalletKey = user.WalletKey,
                    Balance = user.Balance,
                    TotalDeposited = user.TotalDeposited,
                    LimitKeysToCreate = user.LimitKeysToCreate,
                    NoOfCreatedKeys = await _unitOfWork.ProxyKeysRepository.CountByFilterAsync(x => x.UserId == user.Id)
                }),
                Total = await _unitOfWork.UserRepository.CountByFilterAsync()
            });
        }

        [HttpGet]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(RequestUserModel model)
        {
            var user = await _unitOfWork.UserRepository.GetByIDAsync(model.Id);

            if (user != null)
            {
                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new UserModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        APIKey = user.APIKey,
                        WalletKey = user.WalletKey,
                        Balance = user.Balance,
                        TotalDeposited = user.TotalDeposited,
                        LimitKeysToCreate = user.LimitKeysToCreate,
                        NoOfCreatedKeys = await _unitOfWork.ProxyKeysRepository.CountByFilterAsync(p => p.UserId == user.Id)
                    }
                });
            }

            return BadRequest("User not found");
        }

        [HttpPost]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("")]
        public async Task<IActionResult> AddUser(AddUserModel model)
        {
            var user = new UserEntity()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                APIKey = await _userService.GenerateUserAPIKeyAsync(),
                WalletKey = await _userService.GenerateUserWalletKeyAsync(),
                TwoFactorEnabled = true,
                LimitKeysToCreate = model.LimitKeysToCreate > 0 ? model.LimitKeysToCreate : int.Parse(_configuration["LimitKeysToCreate"])
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel
                {
                    Status = "Error",
                    Message = "User creation failed! Please check user details and try again.",
                    Data = result.Errors
                });
            }

            if (await _roleManager.RoleExistsAsync(UserRolesConstant.User))
            {
                await _userManager.AddToRoleAsync(user, UserRolesConstant.User);
            }

            return Ok(new ResponseModel
            {
                Status = "Success",
                Data = new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    APIKey = user.APIKey,
                    WalletKey = user.WalletKey,
                    Balance = user.Balance,
                    TotalDeposited = user.TotalDeposited,
                    LimitKeysToCreate = user.LimitKeysToCreate,
                    NoOfCreatedKeys = 0
                }
            });
        }

        [HttpPatch]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("")]
        public async Task<IActionResult> UpdateUser(UpdateUserModel model)
        {
            string id = model.Id.ToString();
            var user = await _unitOfWork.UserRepository.GetByIDAsync(id);

            if (user != null)
            {
                user.UserName = model.UserName;
                user.Email = model.Email;

                if (model.LimitKeysToCreate > 0)
                {
                    user.LimitKeysToCreate = model.LimitKeysToCreate;
                }

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new UserModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        APIKey = user.APIKey,
                        WalletKey = user.WalletKey,
                        Balance = user.Balance,
                        TotalDeposited = user.TotalDeposited,
                        LimitKeysToCreate = user.LimitKeysToCreate,
                        NoOfCreatedKeys = await _unitOfWork.ProxyKeysRepository.CountByFilterAsync(p => p.UserId == user.Id)
                    }
                });
            }

            return BadRequest("User not found");
        }

        [HttpDelete]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserModel model)
        {
            var list = await _unitOfWork.UserRepository.GetAsync(x => model.Ids.Contains(x.Id));
            if (list != null && list.Count > 0)
            {
                _unitOfWork.UserRepository.DeleteList(list);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Message = "Delete user successfully!"
                });
            }

            return BadRequest("User not found");
        }

        [HttpPatch]
        [Route("DepositMoneyForUser")]
        [Authorize(Roles = UserRolesConstant.Admin)]
        public async Task<IActionResult> DepositMoneyForUser(DepositMoneyForUserModel model)
        {
            var user = await _unitOfWork.UserRepository.GetByFilterAsync(x => x.UserName == model.UserName);

            if (user != null)
            {
                user.Balance += model.Amount;
                user.TotalDeposited += model.Amount;

                _unitOfWork.UserRepository.Update(user);

                // Update wallet history
                await _unitOfWork.WalletHistoryRepository.InsertAsync(new WalletHistoryEntity(await GetCurrentUser())
                {
                    UserId = user.Id,
                    Value = model.Amount,
                    CreatedDate = DateTime.UtcNow,
                    Note = string.IsNullOrWhiteSpace(model.Note) ? $"Admin nap tien cho user" : model.Note,
                });

                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new UserModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        APIKey = user.APIKey,
                        WalletKey = user.WalletKey,
                        Balance = user.Balance,
                        TotalDeposited = user.TotalDeposited,
                        LimitKeysToCreate = user.LimitKeysToCreate,
                        NoOfCreatedKeys = await _unitOfWork.ProxyKeysRepository.CountByFilterAsync(p => p.UserId == user.Id)
                    }
                });
            }

            return BadRequest("User not found");
        }
    }
}
