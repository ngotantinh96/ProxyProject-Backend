using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.Models.Entities;
using ProxyProject_Backend.Models.Response;
using System.Security.Claims;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;

        public UserController(
            UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("GetMe")]
        public async Task<IActionResult> GetMe()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                return Ok(new UserInfoModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    APIKey = user.APIKey,
                    WalletKey = user.WalletKey
                });
            }

            return BadRequest("Empty User");
        }
    }
}
