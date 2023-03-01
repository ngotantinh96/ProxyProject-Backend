using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
using ProxyProject_Backend.Models.Response;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ApiBaseController
    {
        public UserController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager) : base(context, userManager)
        {
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
                        WalletKey = user.WalletKey
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
    }
}
