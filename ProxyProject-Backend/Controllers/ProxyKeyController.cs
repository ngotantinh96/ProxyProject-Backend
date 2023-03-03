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
    [Route("api/[controller]")]
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

                if(proxyKeyPlan != null)
                {
                    var totalOrderedAmount = model.NoOfKeys * model.NoOfDates * proxyKeyPlan.Price;

                    if (totalOrderedAmount <= user.Balance)
                    {
                        var noOfCreatedKeys = await _unitOfWork.ProxyKeysRepository.CountByFilterAsync(x => x.UserId == user.Id);

                        if ((noOfCreatedKeys + model.NoOfKeys) > user.LimitKeysToCreate)
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
                                WalletKey = user.WalletKey,
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

        [HttpGet]
        [Route("GetPurchasedProxyKeys")]
        public async Task<IActionResult> GetPurchasedProxyKeys([FromQuery]GetPurchasedProxyKeysModel model)
        {
            var user = await GetCurrentUser();

            if (user != null)
            {
                var purchasedProxyKeys = await _unitOfWork.ProxyKeysRepository
                    .GetAsync(x => x.UserId == user.Id, x => x.OrderByDescending(p => p.ExpireDate), "ProxyKeyPlan", model.Page, model.Take);

                var proxyKeys = purchasedProxyKeys.Select(x => new ProxyKeysModel
                {
                    Id = x.Id,
                    ProxyKeyPlan = x.ProxyKeyPlan?.Name,
                    ProxyKey = x.Key,
                    ExpireDate = x.ExpireDate,
                    Status = x.ExpireDate < DateTime.UtcNow ? "Key expired!" : "Key working!",
                    Note = x.Note
                });

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = proxyKeys
                });
            }

            return BadRequest("Empty User");
        }
    }
}
