using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
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
        [AllowAnonymous]
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
                        LimitPage= globalConfiguration.LimitPage,
                        ProxyChangeTime = globalConfiguration.ProxyChangeTime
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
        [Route("")]
        public async Task<IActionResult> UpdateSetting([FromBody] ConfigurationRequestModel model)
        {
            var globalConfiguration = await _unitOfWork.GlobalConfigurationRepository.GetByFilterAsync(x => true);

            if (globalConfiguration != null)
            {
                globalConfiguration.TwoFactorEnabled = model.TwoFactAuthen;
                globalConfiguration.LimitPage= model.LimitPage;
                globalConfiguration.ProxyChangeTime = model.ProxyChangeTime;
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
