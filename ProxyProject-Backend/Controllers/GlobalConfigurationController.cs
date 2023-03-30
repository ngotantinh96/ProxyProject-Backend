using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.Response;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/global-config")]
    [ApiController]
    [Authorize(Roles = UserRolesConstant.Admin)]
    public class GlobalConfigurationController : ApiBaseController
    {

        public GlobalConfigurationController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager
            ) : base(context, userManager)
        {
        }

        [HttpGet]
        [Route("GetGlobalConfiguration")]
        public async Task<IActionResult> GetGlobalConfiguration()
        {
            var globalConfiguration = await _unitOfWork.GlobalConfigurationRepository.GetByFilterAsync(x => true);

            if (globalConfiguration != null)
            {
                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new GlobalConfigurationModel
                    {
                        TwoFactorEnabled = globalConfiguration.TwoFactorEnabled,
                    }
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel
            {
                Status = "Error",
                Message = "Error happened. Please try again."
            });
        }

        [HttpPatch]
        [Route("ToggleTwoFactor")]
        public async Task<IActionResult> ToggleTwoFactor()
        {
            var globalConfiguration = await _unitOfWork.GlobalConfigurationRepository.GetByFilterAsync(x => true);

            if (globalConfiguration != null)
            {
                globalConfiguration.TwoFactorEnabled = !globalConfiguration.TwoFactorEnabled;
                _unitOfWork.GlobalConfigurationRepository.Update(globalConfiguration);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new GlobalConfigurationModel
                    {
                        TwoFactorEnabled = globalConfiguration.TwoFactorEnabled,
                    }
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel
            {
                Status = "Error",
                Message = "Error happened. Please try again."
            });
        }

        [HttpPatch]
        [Route("UpdatePageLimit")]
        public async Task<IActionResult> UpdatePageLimit(int limitPage)
        {
            var globalConfiguration = await _unitOfWork.GlobalConfigurationRepository.GetByFilterAsync(x => true);

            if (globalConfiguration != null && limitPage > 0 && limitPage <= 150)
            {
                globalConfiguration.LimitPage = limitPage;
                _unitOfWork.GlobalConfigurationRepository.Update(globalConfiguration);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new GlobalConfigurationModel
                    {
                        TwoFactorEnabled = globalConfiguration.TwoFactorEnabled,
                    }
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel
            {
                Status = "Error",
                Message = "Error happened. Please try again."
            });
        }
    }
}
