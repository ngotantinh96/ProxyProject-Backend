using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.Response;
using ProxyProject_Backend.Services.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProxyProject_Backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class IntegrationController : ApiBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IProxyKeyService _proxyKeyService;

        public IntegrationController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager,
            IConfiguration configuration,
            IProxyKeyService proxyKeyService
            ) : base(context, userManager)
        {
            _configuration = configuration;
            _proxyKeyService = proxyKeyService;
        }

        [HttpGet]
        [Route("ChangeProxy")]
        public async Task<IActionResult> ChangeProxy(string proxyKey)
        {
            var proxyKeyInfo = await _unitOfWork.ProxyKeysRepository.GetByFilterAsync(x => x.Key == proxyKey);
            

            if (proxyKeyInfo != null)
            {
                if (proxyKeyInfo.ExpireDate <= DateTime.UtcNow)
                {
                    return BadRequest(new IntegrationCommonResponseModel
                    {
                        Success = false,
                        Description = "Proxy key is expire!"
                    });
                }
                    var proxyPlan = await _unitOfWork.ProxyKeyPlansRepository.GetByIDAsync(proxyKeyInfo.ProxyKeyPlanId);

                if(proxyPlan != null)
                {
                    var proxy = _unitOfWork.ProxyRepository
                        .GetAsync(x => x.UsingByKey == proxyKeyInfo.Key, x => x.OrderByDescending(_ =>_.StartUsingTime), string.Empty, 0 ,1).Result.FirstOrDefault();

                    if (proxy != null && proxy.EndUsingTime > DateTime.UtcNow)
                    {
                        return Ok(new IntegrationResponseModel
                        {
                            Success = true,
                            Description = string.Empty,
                            Data = new IntegrationProxyModel
                            {
                                Proxy = proxy.Proxy,
                                Country = proxyPlan.Name,
                                NextChange = double.Parse(_configuration["ProxyChangeTime"]),
                                Timeout = (proxy.EndUsingTime - DateTime.UtcNow).Value.TotalSeconds
                            }
                        });
                    }
                    else
                    {
                        var proxyHistories = await _unitOfWork.ProxyHistoryRepository.GetAsync(x => x.UserId == proxyKeyInfo.UserId
                            && x.UsedTime.Date == DateTime.UtcNow.Date);

                        var proxies = await _unitOfWork.ProxyRepository.GetAsync(x => x.ProxyKeyPlanId == proxyPlan.Id
                                && !proxyHistories.Select(x => x.ProxyId).Any(p => p == x.Id)
                                && (!x.StartUsingTime.HasValue || x.EndUsingTime <= DateTime.UtcNow), x => x.OrderBy(p => p.StartUsingTime), "", 0, 1);
                        if (!proxies.Any())
                        {
                            proxies = await _unitOfWork.ProxyRepository.GetAsync(x => x.ProxyKeyPlanId == proxyPlan.Id
                                && (!x.StartUsingTime.HasValue || x.EndUsingTime <= DateTime.UtcNow), x => x.OrderBy(p => p.StartUsingTime), "", 0, 1);
                        }
                        if (proxies.Any())
                        {
                            var proxyChangeTime = double.Parse(_configuration["ProxyChangeTime"]);

                            var startUsingTime = DateTime.UtcNow;
                            var endUsingTime = startUsingTime.AddSeconds(proxyChangeTime);
                            proxyKeyInfo.StartUsingTime = startUsingTime;
                            proxyKeyInfo.EndUsingTime = endUsingTime;
                            _unitOfWork.ProxyKeysRepository.Update(proxyKeyInfo);

                            proxy = proxies.FirstOrDefault();
                            proxy.StartUsingTime = startUsingTime;
                            proxy.EndUsingTime = endUsingTime;
                            proxy.UsingByKey = proxyKeyInfo.Key;

                            await _unitOfWork.ProxyHistoryRepository.InsertAsync(new ProxyHistoryEntity
                            {
                                ProxyId = proxy.Id,
                                UsedTime = DateTime.UtcNow,
                                UserId = proxyKeyInfo.UserId
                            });

                            _unitOfWork.ProxyRepository.Update(proxy);

                            await _unitOfWork.SaveChangesAsync();

                            return Ok(new IntegrationResponseModel
                            {
                                Success = true,
                                Description = string.Empty,
                                Data = new IntegrationProxyModel
                                {
                                    Proxy = proxy.Proxy,
                                    Country = proxyPlan.Name,
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
                }

                return BadRequest(new IntegrationCommonResponseModel
                {
                    Success = false,
                    Description = "Proxy country not found!"
                });
            }

            return BadRequest(new IntegrationCommonResponseModel
            {
                Success = false,
                Description = "Wrong proxy key!"
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
                if (proxyKeyInfo.ExpireDate <= DateTime.UtcNow)
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
        [Route("GetUserKeys")]
        public async Task<IActionResult> GetUserKeys(string apiKey)
        {
            var user = await _unitOfWork.UserRepository.GetByFilterAsync(x => x.APIKey == apiKey);

            if (user != null)
            {
                var proxyKeys = await _unitOfWork.ProxyKeysRepository.GetAsync(x => x.UserId == user.Id,
                    x => x.OrderByDescending(p => p.ExpireDate), "ProxyKeyPlan");

                return Ok(new IntegrationResponseModel
                {
                    Success = true,
                    Description = string.Empty,
                    Data = proxyKeys.Select(x => new IntegrationUserProxyKeyModel
                    {
                        Key = x.Key,
                        Country = x.ProxyKeyPlan?.Name,
                        DateExpired = x.ExpireDate,
                        Status = x.ExpireDate > DateTime.UtcNow ? EnumStatusKey.WORKING : EnumStatusKey.EXPIRED,
                        Description = x.ExpireDate > DateTime.UtcNow ? EnumStatusKey.WORKING.ToString() : EnumStatusKey.EXPIRED.ToString(),
                    })
                });
            }

            return BadRequest(new IntegrationCommonResponseModel
            {
                Success = false,
                Description = "Wrong user api key!"
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

        [HttpGet]
        [Route("GetProxyCountries")]
        public async Task<IActionResult> GetProxyCountries()
        {
            var proxyCountries = await _unitOfWork.ProxyKeyPlansRepository.GetAsync();

            return Ok(new IntegrationResponseModel
            {
                Success = true,
                Description = string.Empty,
                Data = proxyCountries.Select(x => new IntegrationProxyCountryModel
                {
                    CountryId = x.Id,
                    Country = x.Name,
                    Price = x.Price,
                    PriceUnit = x.PriceUnit
                })
            });
        }

        [HttpPost]
        [Route("OrderKeys")]
        public async Task<IActionResult> OrderKeys(string apiKey, int quantity, int days, Guid countryId, string referer)
        {
            var user = await _unitOfWork.UserRepository.GetByFilterAsync(x => x.APIKey == apiKey);

            if (user != null)
            {
                if (quantity > 0)
                {
                    if (days > 0)
                    {
                        var proxyKeyCountry = await _unitOfWork.ProxyKeyPlansRepository.GetByIDAsync(countryId);

                        if (proxyKeyCountry != null)
                        {
                            var totalOrderedAmount = quantity * days * proxyKeyCountry.Price;

                            if (totalOrderedAmount <= user.Balance)
                            {
                                var noOfCreatedKeys = await _unitOfWork.ProxyKeysRepository.CountByFilterAsync(x => x.UserId == user.Id);

                                if ((noOfCreatedKeys + quantity) <= user.LimitKeysToCreate)
                                {
                                    // Perform orders
                                    var listOrderedKey = new List<ProxyKeysEntity>();

                                    for (int i = 0; i < quantity; i++)
                                    {
                                        listOrderedKey.Add(new ProxyKeysEntity
                                        {
                                            Key = await _proxyKeyService.GenerateProxyKeyAsync(),
                                            ProxyKeyPlanId = proxyKeyCountry.Id,
                                            ExpireDate = DateTime.UtcNow.AddDays(days),
                                            UserId = user.Id
                                        });
                                    }

                                    await _unitOfWork.ProxyKeysRepository.InsertListAsync(listOrderedKey);

                                    // Update user balance
                                    user.Balance -= totalOrderedAmount;
                                    _unitOfWork.UserRepository.Update(user);

                                    // Update wallet history
                                    await _unitOfWork.WalletHistoryRepository.InsertAsync(new
                                        WalletHistoryEntity(await GetCurrentUser())
                                    {
                                        UserId = user.Id,
                                        Value = -totalOrderedAmount,
                                        CreatedDate = DateTime.UtcNow,
                                        Note = $"Mua {quantity} keys - {days} ngay. {proxyKeyCountry.Name}"
                                    });

                                    await _unitOfWork.SaveChangesAsync();

                                    return Ok(new IntegrationResponseModel
                                    {
                                        Success = true,
                                        Description = string.Empty,
                                        Data = listOrderedKey.Select(x => new IntegrationUserProxyKeyModel
                                        {
                                            Key = x.Key,
                                            Country = proxyKeyCountry.Name,
                                            DateExpired = x.ExpireDate,
                                            Status = x.ExpireDate > DateTime.UtcNow ? EnumStatusKey.WORKING : EnumStatusKey.EXPIRED,
                                            Description = x.ExpireDate > DateTime.UtcNow ? EnumStatusKey.WORKING.ToString() : EnumStatusKey.EXPIRED.ToString(),
                                        })
                                    });
                                }

                                return BadRequest(new IntegrationCommonResponseModel
                                {
                                    Success = false,
                                    Description = "Exceed limit keys to created!"
                                });
                            }

                            return BadRequest(new IntegrationCommonResponseModel
                            {
                                Success = false,
                                Description = "Balance is not enough!"
                            });
                        }

                        return BadRequest(new IntegrationCommonResponseModel
                        {
                            Success = false,
                            Description = "Wrong country id!"
                        });
                    }

                    return BadRequest(new IntegrationCommonResponseModel
                    {
                        Success = false,
                        Description = "Days must greater than 0!"
                    });
                }

                return BadRequest(new IntegrationCommonResponseModel
                {
                    Success = false,
                    Description = "Quantity must greater than 0!"
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
        public async Task<IActionResult> ExtendKey(string apiKey, string proxyKey, int days, string referer)
        {
            var user = await _unitOfWork.UserRepository.GetByFilterAsync(x => x.APIKey == apiKey);

            if (user != null)
            {
                var proxyKeyInfo = await _unitOfWork.ProxyKeysRepository.GetByFilterAsync(x => x.Key == proxyKey);

                if (proxyKeyInfo != null)
                {
                    if (days > 0)
                    {
                        var proxyKeyCountry = await _unitOfWork.ProxyKeyPlansRepository.GetByIDAsync(proxyKeyInfo.ProxyKeyPlanId);

                        var totalOrderedAmount = proxyKeyCountry.Price * days;

                        if (totalOrderedAmount <= user.Balance)
                        {
                            proxyKeyInfo.ExpireDate = proxyKeyInfo.ExpireDate.AddDays(days);
                            _unitOfWork.ProxyKeysRepository.Update(proxyKeyInfo);

                            // Update user balance
                            user.Balance -= totalOrderedAmount;
                            _unitOfWork.UserRepository.Update(user);

                            // Update wallet history
                            await _unitOfWork.WalletHistoryRepository.InsertAsync(new WalletHistoryEntity(await GetCurrentUser())
                            {
                                UserId = user.Id,
                                Value = -totalOrderedAmount,
                                CreatedDate = DateTime.UtcNow,
                                Note = $"Gia han 1 key - {days} ngay."
                            });

                            await _unitOfWork.SaveChangesAsync();

                            return Ok(new IntegrationResponseModel
                            {
                                Success = true,
                                Description = string.Empty,
                                Data = new IntegrationUserProxyKeyModel
                                {
                                    Key = proxyKeyInfo.Key,
                                    Country = proxyKeyCountry.Name,
                                    DateExpired = proxyKeyInfo.ExpireDate,
                                    Status = proxyKeyInfo.ExpireDate > DateTime.UtcNow ? EnumStatusKey.WORKING : EnumStatusKey.EXPIRED,
                                    Description = proxyKeyInfo.ExpireDate > DateTime.UtcNow ? EnumStatusKey.WORKING.ToString() : EnumStatusKey.EXPIRED.ToString(),
                                }
                            });
                        }

                        return BadRequest("Balance is not enough");
                    }

                    return BadRequest(new IntegrationCommonResponseModel
                    {
                        Success = false,
                        Description = "Days must greater than 0!"
                    });
                }

                return BadRequest(new IntegrationCommonResponseModel
                {
                    Success = false,
                    Description = "Wrong proxy key!"
                });
            }

            return BadRequest(new IntegrationCommonResponseModel
            {
                Success = false,
                Description = "Wrong user api key!"
            });
        }
    }
}
