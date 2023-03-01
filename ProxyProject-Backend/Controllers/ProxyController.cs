using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.Response;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProxyController : ApiBaseController
    {
        public ProxyController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager) : base(context, userManager)
        {
        }

        [HttpGet]
        [Route("GetProxies")]
        public async Task<IActionResult> GetProxies()
        {
            return Ok(new ResponseModel
            {
                Status = "Success",
                Data = await _unitOfWork.ProxyRepository.GetAsync()
            });
        }
    }
}
