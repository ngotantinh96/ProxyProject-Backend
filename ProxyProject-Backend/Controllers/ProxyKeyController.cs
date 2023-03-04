using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
using ProxyProject_Backend.Models.Response;
using ProxyProject_Backend.Services.Interface;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/userproxies")]
    [ApiController]
    [Authorize]
    public class ProxyKeyController : ApiBaseController
    {
        private readonly IProxyKeyService _proxyKeyService;
        public ProxyKeyController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager,
            IProxyKeyService proxyKeyService
            ) : base(context, userManager)
        {
            _proxyKeyService = proxyKeyService;
        }

        [HttpGet]
        [Route("GetProxyKeyPlans")]
        public async Task<IActionResult> GetProxyKeyPlans()
        {
            return Ok(new ResponseModel
            {
                Status = "Success",
                Data = await _unitOfWork.ProxyKeyPlansRepository.GetAsync()
            });
        }
        [HttpPost]
        [Route("OrderProxyKeys")]
        public async Task<IActionResult> OrderProxyKeys(OrderProxyKeysModel model)
        {
            var user = await GetCurrentUser();

            if (user != null)
            {

                var proxyKeyPlan = await _unitOfWork.ProxyKeyPlansRepository.GetByIDAsync(model.ProxyKeyPlanId);

                if (proxyKeyPlan != null)
                {
                    var totalOrderedAmount = model.NoOfKeys * model.NoOfDates * proxyKeyPlan.Price;

                    if (totalOrderedAmount <= user.Balance)
                    {
                        var noOfCreatedKeys = await _unitOfWork.ProxyKeysRepository.CountByFilterAsync(x => x.UserId == user.Id);

                        if ((noOfCreatedKeys + model.NoOfKeys) <= user.LimitKeysToCreate)
                        {
                            // Perform orders
                            var listOrderedKey = new List<ProxyKeysEntity>();

                            for (int i = 0; i < model.NoOfKeys; i++)
                            {
                                listOrderedKey.Add(new ProxyKeysEntity
                                {
                                    Key = await _proxyKeyService.GenerateProxyKeyAsync(),
                                    ProxyKeyPlanId = proxyKeyPlan.Id,
                                    ExpireDate = DateTime.UtcNow.AddDays(model.NoOfDates),
                                    UserId = user.Id
                                });
                            }

                            await _unitOfWork.ProxyKeysRepository.InsertListAsync(listOrderedKey);

                            // Update user balance
                            user.Balance -= totalOrderedAmount;
                            _unitOfWork.UserRepository.Update(user);

                            // Update wallet history
                            await _unitOfWork.WalletHistoryRepository.InsertAsync(new WalletHistoryEntity
                            {
                                UserId = user.Id,
                                Value = -totalOrderedAmount,
                                CreatedDate = DateTime.UtcNow,
                                Note = $"Mua {model.NoOfKeys} keys - {model.NoOfDates} ngay. {proxyKeyPlan.Name}"
                            });

                            await _unitOfWork.SaveChangesAsync();

                            return Ok(new ResponseModel
                            {
                                Status = "Success",
                                Data = listOrderedKey.Select(x => x.Key).ToList()
                            });
                        }

                        return BadRequest("Exceed limit keys to created");
                    }
                    return BadRequest("Balance is not enough");
                }

                return BadRequest("Proxy Plan not found");
            }

            return BadRequest("Empty User");
        }

        [HttpPost]
        [Route("ExtendProxyKeys")]
        public async Task<IActionResult> ExtendProxyKeys(ExtendProxyKeysModel model)
        {
            var user = await GetCurrentUser();

            if (user != null)
            {
                var proxyKeys = await _unitOfWork.ProxyKeysRepository
                    .GetAsync(x => x.UserId == user.Id && model.Keys.Contains(x.Key), null, "ProxyKeyPlan");

                if (proxyKeys.Any())
                {
                    var totalOrderedAmount = proxyKeys.Sum(x => x.ProxyKeyPlan.Price * model.NoOfDates);

                    if (totalOrderedAmount <= user.Balance)
                    {
                        // Perform orders
                        foreach (var proxyKey in proxyKeys)
                        {
                            proxyKey.ExpireDate = proxyKey.ExpireDate.AddDays(model.NoOfDates);
                        }

                        _unitOfWork.ProxyKeysRepository.UpdateList(proxyKeys);

                        // Update user balance
                        user.Balance -= totalOrderedAmount;
                        _unitOfWork.UserRepository.Update(user);

                        // Update wallet history
                        await _unitOfWork.WalletHistoryRepository.InsertAsync(new WalletHistoryEntity
                        {
                            UserId = user.Id,
                            Value = -totalOrderedAmount,
                            CreatedDate = DateTime.UtcNow,
                            Note = $"Mua {proxyKeys.Count()} keys - {model.NoOfDates} ngay."
                        });

                        await _unitOfWork.SaveChangesAsync();

                        return Ok(new ResponseModel
                        {
                            Status = "Success",
                            Data = model.Keys
                        });
                    }
                    return BadRequest("Balance is not enough");
                }

                return BadRequest("Keys not found");
            }

            return BadRequest("Empty User");
        }

        [HttpDelete]
        [Route("DeleteProxyKeys")]
        public async Task<IActionResult> DeleteProxyKeys(ExtendProxyKeysModel model)
        {
            var user = await GetCurrentUser();

            if (user != null)
            {
                var proxyKeys = await _unitOfWork.ProxyKeysRepository.GetAsync(x => x.UserId == user.Id && model.Keys.Contains(x.Key));

                if (proxyKeys.Any())
                {
                    _unitOfWork.ProxyKeysRepository.DeleteList(proxyKeys);
                    await _unitOfWork.SaveChangesAsync();

                    return Ok(new ResponseModel
                    {
                        Status = "Success",
                        Message = "Remove keys successfully!"
                    });
                }

                return BadRequest("Keys not found");
            }

            return BadRequest("Empty User");
        }

        [HttpPost]
        [Route("TakeNoteProxyKey")]
        public async Task<IActionResult> TakeNoteProxyKey(TakeNoteProxyKeyModel model)
        {
            var user = await GetCurrentUser();

            if (user != null)
            {
                var proxyKey = await _unitOfWork.ProxyKeysRepository.GetByFilterAsync(x => x.UserId == user.Id && x.Key == model.Key);

                if (proxyKey != null)
                {
                    proxyKey.Note = model.Note;

                    _unitOfWork.ProxyKeysRepository.Update(proxyKey);
                    await _unitOfWork.SaveChangesAsync();

                    return Ok(new ResponseModel
                    {
                        Status = "Success",
                        Data = new ProxyKeyModel
                        {
                            Id = proxyKey.Id,
                            ProxyKeyPlan = proxyKey.ProxyKeyPlan?.Name,
                            ProxyKey = proxyKey.Key,
                            ExpireDate = proxyKey.ExpireDate,
                            Status = proxyKey.ExpireDate < DateTime.UtcNow ? EnumStatusKey.WORKING : EnumStatusKey.EXPIRED,
                            Note = proxyKey.Note
                        }
                    });
                }

                return BadRequest("Key not found");
            }

            return BadRequest("Empty User");
        }

        [HttpGet]
        [Route("purchased")]
        public async Task<IActionResult> GetPurchasedProxyKeys([FromQuery] GetListPagingModel model)
        {
            var user = await GetCurrentUser();

            if (user != null)
            {
                var purchasedProxyKeys = await _unitOfWork.ProxyKeysRepository
                    .GetAsync(x => x.UserId == user.Id, x => x.OrderByDescending(p => p.ExpireDate), "ProxyKeyPlan", model.PageIndex, model.PageSize);

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = purchasedProxyKeys.Select(x => new ProxyKeyModel
                    {
                        Id = x.Id,
                        ProxyKeyPlan = x.ProxyKeyPlan?.Name,
                        ProxyKey = x.Key,
                        ExpireDate = x.ExpireDate,
                        Status = x.ExpireDate < DateTime.UtcNow ? EnumStatusKey.WORKING : EnumStatusKey.EXPIRED,
                        Note = x.Note
                    }),
                    Total = await _unitOfWork.ProxyKeysRepository.CountByFilterAsync(x => x.UserId == user.Id)
            });
            }

            return BadRequest("Empty User");
        }
    }
}
