using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
using ProxyProject_Backend.Models.Response;
namespace ProxyProject_Backend.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    [Authorize]
    public class WalletController : ApiBaseController
    {
        public WalletController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager
            ) : base(context, userManager)
        {
        }

        [HttpGet]
        [Route("GetWalletHistory")]
        public async Task<IActionResult> GetWalletHistory([FromQuery] GetWalletHistoryModel model)
        {
            var user = await GetCurrentUser();

            if (user != null)
            {
                var walletHistory = await _unitOfWork.WalletHistoryRepository
                    .GetAsync(x => x.UserId == user.Id, x => x.OrderByDescending(p => p.CreatedDate), "", model.PageIndex, model.PageSize);

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = walletHistory.Select(x => new WalletHistoryModel
                    {
                        Id = x.Id,
                        CreatedDate = x.CreatedDate,
                        Value = x.Value,
                        Note = x.Note
                    }),
                    Total = await _unitOfWork.WalletHistoryRepository.CountByFilterAsync(x => x.UserId == user.Id)
                });
            }

            return BadRequest("Empty User");
        }

        [HttpGet]
        [Route("GetWalletInfoByKey")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWalletInfoByKey([FromQuery] GetWalletInfoModel model)
        {
            var user = await _unitOfWork.UserRepository.GetByFilterAsync(x => x.WalletKey == model.WalleyKey);

            if (user != null)
            {
                var walletHistory = await _unitOfWork.WalletHistoryRepository
                    .GetAsync(x => x.UserId == user.Id, x => x.OrderByDescending(p => p.CreatedDate), "", model.PageIndex, model.PageSize);

                var count = await _unitOfWork.WalletHistoryRepository.CountByFilterAsync(x => x.UserId == user.Id);

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new WalletInfoModel
                    {
                        Balance = user.Balance,
                        TotalDeposited = user.TotalDeposited,
                        History = walletHistory.Select(x => new WalletHistoryModel
                        {
                            Id = x.Id,
                            CreatedDate = x.CreatedDate,
                            Value = x.Value,
                            Note = x.Note
                        }).ToList()
                    },
                    Total = await _unitOfWork.WalletHistoryRepository.CountByFilterAsync(x => x.UserId == user.Id)
                });
            }

            return BadRequest("Wallet not found");
        }
    }
}
