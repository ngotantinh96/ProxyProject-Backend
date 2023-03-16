using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.Response;
namespace ProxyProject_Backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class IntegrationController : ApiBaseController
    {
        private readonly IConfiguration _configuration;
        public IntegrationController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager,
            IConfiguration configuration
            ) : base(context, userManager)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("ChangeProxy")]
        public async Task<IActionResult> ChangeProxy(string apiKey)
        {
            var user = await _unitOfWork.UserRepository.GetByFilterAsync(x => x.APIKey == apiKey);

            if (user != null)
            {
                var proxyKeys = await _unitOfWork.ProxyKeysRepository.GetAsync(x => x.UserId == user.Id && x.ExpireDate > DateTime.UtcNow
                    && (!x.StartUsingTime.HasValue || x.EndUsingTime <= DateTime.UtcNow), x => x.OrderBy(p => p.StartUsingTime), "ProxyKeyPlan", 0, 1);

                if (proxyKeys.Any())
                {
                    var proxyKey = proxyKeys.FirstOrDefault();
                    var proxies = await _unitOfWork.ProxyRepository.GetAsync(x => x.ProxyKeyPlanId == proxyKey.ProxyKeyPlanId 
                        && (!x.StartUsingTime.HasValue || x.EndUsingTime <= DateTime.UtcNow), x => x.OrderBy(p => p.StartUsingTime), "", 0, 1);

                    if (proxies.Any())
                    {
                        var proxyChangeTime = double.Parse(_configuration["ProxyChangeTime"]);

                        var startUsingTime = DateTime.UtcNow;
                        var endUsingTime = startUsingTime.AddSeconds(proxyChangeTime);
                        proxyKey.StartUsingTime = startUsingTime;
                        proxyKey.EndUsingTime = endUsingTime;
                        _unitOfWork.ProxyKeysRepository.Update(proxyKey);

                        var proxy = proxies.FirstOrDefault();
                        proxy.StartUsingTime = startUsingTime;
                        proxy.EndUsingTime = endUsingTime;
                        proxy.UsingByKey = proxyKey.Key;

                        _unitOfWork.ProxyRepository.Update(proxy);

                        await _unitOfWork.SaveChangesAsync(); 

                        return Ok(new IntegrationResponseModel
                        {
                            Success = true,
                            Description = string.Empty,
                            Data = new IntegrationProxyModel
                            {
                                Proxy = proxy.Proxy,
                                Country = proxyKey.ProxyKeyPlan.Name,
                                NextChange = proxyChangeTime,
                                Timeout = proxyChangeTime
                            }
                        });
                    }

                    return BadRequest(new IntegrationCommonResponseModel
                    {
                        Success = false,
                        Description = "All proxies are in use!"
                    });
                }

                return BadRequest(new IntegrationCommonResponseModel
                {
                    Success = false,
                    Description = "All proxy keys are in use or proxy keys are not created!"
                });
            }

            return BadRequest(new IntegrationCommonResponseModel
            {
                Success = false,
                Description = "Wrong user api key!"
            });
        }

        [HttpGet]
        [Route("GetProxy")]
        public async Task<IActionResult> GetProxy(string proxyKey)
        {
            var proxy = await _unitOfWork.ProxyRepository.GetByFilterAsync(x => x.UsingByKey == proxyKey);

            if (proxy != null)
            {
                var proxyPlan = await _unitOfWork.ProxyKeyPlansRepository.GetByIDAsync(proxy.ProxyKeyPlanId);
                
                return Ok(new IntegrationResponseModel
                {
                    Success = true,
                    Description = string.Empty,
                    Data = new IntegrationProxyModel
                    {
                        Proxy = proxy.Proxy,
                        Country = proxyPlan?.Name,
                        NextChange = double.Parse(_configuration["ProxyChangeTime"]),
                        Timeout = (proxy.EndUsingTime - DateTime.UtcNow).Value.TotalSeconds >= 0 ? (proxy.EndUsingTime - DateTime.UtcNow).Value.TotalSeconds : 0
                    }
                });
            }

            return BadRequest(new IntegrationCommonResponseModel
            {
                Success = false,
                Description = "Proxy not found! Please try again or contact us."
            });
        }

        [HttpGet]
        [Route("GetKeyInfo")]
        public async Task<IActionResult> GetKeyInfo(string proxyKey)
        {
            var proxyKeyInfo = await _unitOfWork.ProxyKeysRepository.GetByFilterAsync(x => x.Key == proxyKey);

            if (proxyKeyInfo != null)
            {
                return Ok(new IntegrationResponseModel
                {
                    Success = true,
                    Description = string.Empty,
                    Data = new IntegrationProxyKeyModel
                    {
                        DateExpired = proxyKeyInfo.ExpireDate,
                        IsInUse = proxyKeyInfo.EndUsingTime < DateTime.UtcNow,
                    }
                });
            }

            return BadRequest(new IntegrationCommonResponseModel
            {
                Success = false,
                Description = "Wrong proxy key!"
            });
        }

        [HttpGet]
        [Route("DeleteKey")]
        public async Task<IActionResult> DeleteKey(string proxyKey)
        {
            var proxyKeyInfo = await _unitOfWork.ProxyKeysRepository.GetByFilterAsync(x => x.Key == proxyKey);

            if (proxyKeyInfo != null)
            {
                if(proxyKeyInfo.ExpireDate <= DateTime.UtcNow)
                {
                    _unitOfWork.ProxyKeysRepository.Delete(proxyKeyInfo.Id);
                    await _unitOfWork.SaveChangesAsync();

                    return Ok(new IntegrationCommonResponseModel
                    {
                        Success = true,
                        Description = "Key deleted!"
                    });
                }

                return BadRequest(new IntegrationCommonResponseModel
                {
                    Success = false,
                    Description = "Key not expired yet!"
                });
            }

            return BadRequest(new IntegrationCommonResponseModel
            {
                Success = false,
                Description = "Wrong proxy key!"
            });
        }

        [HttpGet]
        [Route("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo(string apiKey)
        {
            var user = await _unitOfWork.UserRepository.GetByFilterAsync(x => x.APIKey == apiKey);

            if (user != null)
            {
                return Ok(new IntegrationResponseModel
                {
                    Success = true,
                    Description = string.Empty,
                    Data = new IntegrationUserInfoModel
                    {
                        MaxKey = user.LimitKeysToCreate,
                        Balance = user.Balance,
                        TotalDeposited = user.TotalDeposited,
                        WalletKey = user.WalletKey,
                    }
                });
            }

            return BadRequest(new IntegrationCommonResponseModel
            {
                Success = false,
                Description = "Wrong user api key!"
            });
        }

        [HttpPost]
        [Route("ExtendKey")]
        public async Task<IActionResult> ExtendKey(string apiKey)
        {
            var user = await _unitOfWork.UserRepository.GetByFilterAsync(x => x.APIKey == apiKey);

            if (user != null)
            {
                //var proxyKeys = await _unitOfWork.ProxyKeysRepository
                //    .GetAsync(x => x.UserId == user.Id && model.Keys.Contains(x.Key), null, "ProxyKeyPlan");

                //if (proxyKeys.Any())
                //{
                //    var totalOrderedAmount = proxyKeys.Sum(x => x.ProxyKeyPlan.Price * model.NoOfDates);

                //    if (totalOrderedAmount <= user.Balance)
                //    {
                //        // Perform orders
                //        foreach (var proxyKey in proxyKeys)
                //        {
                //            proxyKey.ExpireDate = proxyKey.ExpireDate.AddDays(model.NoOfDates);
                //        }

                //        _unitOfWork.ProxyKeysRepository.UpdateList(proxyKeys);

                //        // Update user balance
                //        user.Balance -= totalOrderedAmount;
                //        _unitOfWork.UserRepository.Update(user);

                //        // Update wallet history
                //        await _unitOfWork.WalletHistoryRepository.InsertAsync(new WalletHistoryEntity
                //        {
                //            UserId = user.Id,
                //            Value = -totalOrderedAmount,
                //            CreatedDate = DateTime.UtcNow,
                //            Note = $"Mua {proxyKeys.Count()} keys - {model.NoOfDates} ngay."
                //        });

                //        await _unitOfWork.SaveChangesAsync();

                //        return Ok(new ResponseModel
                //        {
                //            Status = "Success",
                //            Data = model.Keys
                //        });
                //    }
                //    return BadRequest("Balance is not enough");
                //}

                return BadRequest("Keys not found");
            }

            return BadRequest("Empty User");
        }
    }
}
