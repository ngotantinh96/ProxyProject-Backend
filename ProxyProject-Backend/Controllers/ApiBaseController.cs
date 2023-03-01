using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using System.Security.Claims;

namespace ProxyProject_Backend.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        protected readonly UserManager<UserEntity> _userManager;
        protected readonly ApplicationDbContext _context;
        protected UnitOfWork _unitOfWork;

        public ApiBaseController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
            _unitOfWork = new UnitOfWork(context);
        }

        protected async Task<UserEntity> GetCurrentUser()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            return await _userManager.FindByNameAsync(userName);
        }
    }
}
