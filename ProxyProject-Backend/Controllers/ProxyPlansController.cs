using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
using ProxyProject_Backend.Models.Response;
using System.Linq.Expressions;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/proxy-plans")]
    [ApiController]
    [Authorize]
    public class ProxyPlansController : ApiBaseController
    {
        public ProxyPlansController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager
            ) : base(context, userManager)
        {
        }

        [HttpGet]
        [Route("GetProxyPlans")]
        public async Task<IActionResult> GetProxyPlans([FromQuery] GetListPagingModel model)
        {
            Expression<Func<ProxyKeyPlansEntity, bool>> filter = null;

            if (!string.IsNullOrWhiteSpace(model.Keyword))
            {
                filter = (x) => x.Name == model.Keyword;
            }

            var ProxyPlans = await _unitOfWork.ProxyKeyPlansRepository
                     .GetAsync(filter, x => x.OrderBy(p => p.Name), "", model.PageIndex, model.PageSize);

            return Ok(new ResponseModel
            {
                Status = "Success",
                Data = ProxyPlans.Select(x => new ProxyPlanModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Price = x.Price,
                    PriceUnit = x.PriceUnit,
                    Description = x.Description
                }),
                Total = await _unitOfWork.ProxyKeyPlansRepository.CountByFilterAsync()
            });
        }

        [HttpGet]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("GetProxyPlan")]
        public async Task<IActionResult> GetProxyPlan(RequestProxyPlansModel model)
        {
            var proxyPlan = await _unitOfWork.ProxyKeyPlansRepository.GetByIDAsync(model.Id);

            if (proxyPlan != null)
            {
                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new ProxyPlanModel
                    {
                        Id = proxyPlan.Id,
                        Name = proxyPlan.Name,
                        Code = proxyPlan.Code,
                        Price = proxyPlan.Price,
                        PriceUnit = proxyPlan.PriceUnit,
                        Description = proxyPlan.Description
                    }
                });
            }

            return BadRequest("Proxy plan not found");
        }

        [HttpPost]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("AddProxyPlan")]
        public async Task<IActionResult> AddProxyPlan(AddProxyPlanModel model)
        {
            var proxyPlan = await _unitOfWork.ProxyKeyPlansRepository.InsertAsync(new ProxyKeyPlansEntity
            {
                Name = model.Name,
                Code = model.Code,
                Price = model.Price,
                PriceUnit = model.PriceUnit,
                Description = model.Description
            });

            await _unitOfWork.SaveChangesAsync();

            if (proxyPlan != null)
            {
                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new ProxyPlanModel
                    {
                        Id = proxyPlan.Id,
                        Name = proxyPlan.Name,
                        Code = proxyPlan.Code,
                        Price = proxyPlan.Price,
                        PriceUnit = proxyPlan.PriceUnit,
                        Description = proxyPlan.Description
                    }
                });
            }

            return BadRequest("Error when adding proxy plan");

        }

        [HttpPatch]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("UpdateProxyPlan")]
        public async Task<IActionResult> UpdateProxyPlan(UpdateProxyPlanModel model)
        {
            var proxyPlan = await _unitOfWork.ProxyKeyPlansRepository.GetByIDAsync(model.Id);

            if (proxyPlan != null)
            {
                proxyPlan.Name = model.Name;
                proxyPlan.Code = model.Code;
                proxyPlan.Price = model.Price;
                proxyPlan.PriceUnit = model.PriceUnit;
                proxyPlan.Description = model.Description;

                _unitOfWork.ProxyKeyPlansRepository.Update(proxyPlan);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new ProxyPlanModel
                    {
                        Id = proxyPlan.Id,
                        Name = proxyPlan.Name,
                        Code = proxyPlan.Code,
                        Price = proxyPlan.Price,
                        PriceUnit = proxyPlan.PriceUnit,
                        Description = proxyPlan.Description
                    }
                });
            }

            return BadRequest("Proxy plan not found");
        }

        [HttpDelete]
        [Authorize(Roles = UserRolesConstant.Admin)]
        [Route("DeleteProxyPlan")]
        public async Task<IActionResult> DeleteProxyPlan(DeleteProxyKeyPlanRequestModel model)
        {
            var ProxyPlans = await _unitOfWork.ProxyKeyPlansRepository.GetAsync(x => model.Ids.Contains(x.Id));


            if (ProxyPlans != null)
            {
                _unitOfWork.ProxyKeyPlansRepository.DeleteList(ProxyPlans);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Message = "Delete proxy plan successfully!"
                });
            }

            return BadRequest("Proxy plan not found");
        }
    }
}
