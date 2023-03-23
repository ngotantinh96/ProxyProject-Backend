using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
using ProxyProject_Backend.Models.Response;
using ProxyProject_Backend.Utils;
using System.Linq.Expressions;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/proxy")]
    [ApiController]
    [Authorize]
    public class ProxyController : ApiBaseController
    {
        public ProxyController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager
            ) : base(context, userManager)
        {
        }

        [HttpPost]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("UploadProxy")]
        public async Task<IActionResult> UploadProxy([FromForm] UploadProxyModel model)
        {
            if (model.ProxyFile != null && model.ProxyFile.Length > 0)
            {
                if (FileUtils.IsFileExtensionAllowed(model.ProxyFile.FileName, new string[] { ".txt" }))
                {
                    var proxyList = new List<ProxyEntity>();

                    try
                    {
                        using (var reader = new StreamReader(model.ProxyFile.OpenReadStream()))
                        {
                            while (reader.Peek() >= 0)
                            {
                                var proxy = await reader.ReadLineAsync();
                                if (!string.IsNullOrWhiteSpace(proxy))
                                {
                                    proxyList.Add(new ProxyEntity
                                    {
                                        Proxy = proxy,
                                        ProxyKeyPlanId = model.CountryId
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
                    }

                    await _unitOfWork.ProxyRepository.InsertListAsync(proxyList);
                    await _unitOfWork.SaveChangesAsync();

                    if (proxyList.Any())
                    {
                        return Ok(new ResponseModel
                        {
                            Status = "Success",
                            Message = "Proxies uploaded successfully!",
                            Data = proxyList.Select(x => x.Proxy)
                        });
                    }

                    return BadRequest("Cannot save proxy list");
                }

                return BadRequest("File extention is not allowed");
            }

            return BadRequest("File is empty");
        }

        [HttpPost]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("")]
        public async Task<IActionResult> AddProxy([FromBody] AddProxyModel model)
        {
            if (model.Proxies.Any())
            {
                var proxies = model.Proxies.Select(proxy => new ProxyEntity
                {
                    Proxy = proxy,
                    ProxyKeyPlanId = model.CountryId
                }).ToList();

                await _unitOfWork.ProxyRepository.InsertListAsync(proxies);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Message = "Proxies inserted successfully!",
                });
            }

            return BadRequest("Proxy list is empty");
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProxies([FromQuery] GetListPagingModel model)
        {
            Expression<Func<ProxyEntity, bool>> filter = null;
            Expression<Func<ProxyEntity, bool>> isUse = null;
            if (!string.IsNullOrWhiteSpace(model.Keyword))
            { 
                if (model.IsUse != null && model.IsUse > 0)
                {
                    if (model.IsUse == 1) filter = (x) => x.EndUsingTime >= DateTime.UtcNow && x.Proxy.Contains(model.Keyword);
                    else filter = (x) => (x.EndUsingTime  == null || x.EndUsingTime.Value < DateTime.UtcNow) && x.Proxy.Contains(model.Keyword);
                }
                else
                {
                    filter  = (x) => x.Proxy.Contains(model.Keyword);
                }
            }
            else
            {
                if (model.IsUse != null && model.IsUse > 0)
                {
                    if (model.IsUse == 1) filter = (x) => x.EndUsingTime >= DateTime.UtcNow;
                    else filter = (x) => (x.EndUsingTime == null || x.EndUsingTime.Value < DateTime.UtcNow);
                }
            }
            var notifications = await _unitOfWork.ProxyRepository
                     .GetAsync(filter, x => x.OrderBy(p => p.Proxy), "ProxyKeyPlan", model.PageIndex, model.PageSize);

            return Ok(new ResponseModel
            {
                Status = "Success",
                Data = notifications.Select(proxy => new ProxyModel
                {
                    Id = proxy.Id,
                    Proxy = proxy.Proxy,
                    StartUsingTime = proxy.StartUsingTime,
                    EndUsingTime = proxy.EndUsingTime,
                    CountryName = proxy.ProxyKeyPlan.Name,
                    CountryCode = proxy.ProxyKeyPlan.Code,
                    ProxyKeyPlanId= proxy.ProxyKeyPlan.Id,
                    IsUse = proxy.EndUsingTime >= DateTime.UtcNow ? 1: 2
                }),
                Total = await _unitOfWork.ProxyRepository.CountByFilterAsync()
            });
        }

        [HttpGet]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("GetProxy")]
        public async Task<IActionResult> GetProxy(RequestProxyModel model)
        {
            var proxy = await _unitOfWork.ProxyRepository.GetByIDAsync(model.Id);

            if (proxy != null)
            {
                var proxyPlan = proxy.ProxyKeyPlan ?? await _unitOfWork.ProxyKeyPlansRepository.GetByIDAsync(proxy.ProxyKeyPlanId);

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new ProxyModel
                    {
                        Id = proxy.Id,
                        Proxy = proxy.Proxy,
                        StartUsingTime = proxy.StartUsingTime,
                        EndUsingTime = proxy.EndUsingTime,
                        CountryName = proxyPlan.Name,
                        CountryCode = proxyPlan.Code,
                    }
                });
            }

            return BadRequest("Proxy not found");
        }

        [HttpDelete]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("")]
        public async Task<IActionResult> DeleteProxy(DeleteProxyModel model)
        {
            _unitOfWork.ProxyRepository.DeleteList(model.Ids);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ResponseModel
            {
                Status = "Success",
                Message = "Delete proxy successfully!"
            });
        }

        [HttpGet]
        [Route("CountNoOfIPs")]
        public async Task<IActionResult> CountNoOfIPs()
        {
            return Ok(new ResponseModel
            {
                Status = "Success",
                Data = await _unitOfWork.ProxyRepository.CountByFilterAsync()
            });
        }
    }
}
