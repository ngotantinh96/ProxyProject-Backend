using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.Models.RequestModels;
using static Dapper.SqlMapper;
using System.Linq.Expressions;
using ProxyProject_Backend.Models.Response;
using ProxyProject_Backend.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHistoriesController : ApiBaseController
    {
        public TransactionHistoriesController(
            ApplicationDbContext context,
            UserManager<UserEntity> userManager
            ) : base(context, userManager)
        {
        }

        [HttpGet]
        [Authorize(Roles = UserRolesConstant.Admin)]
        public async Task<IActionResult> GetTransactionHistory([FromQuery] GetTransactionHistoryRequestModel request)
        {
            Expression<Func<TransactionHistoryEntity, bool>> filter = null;
            Func<IQueryable<TransactionHistoryEntity>, IOrderedQueryable<TransactionHistoryEntity>> orderBy = (x) => x.OrderByDescending(p => p.TransactionDate);

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                filter = (x) => x.User.UserName == request.Keyword;
            }

            var result = await _unitOfWork.TransactionHistoryRepository.GetAsync(filter, orderBy,"User",request.Page, request.Size ?? 10);

            return Ok(new ResponseModel
            {
                Status = "Success",
                Data = result.Select(x => new TransactionHistoryResponseModel
                {
                    Id = x.Id,
                    TransactionDate = x.TransactionDate,
                    UserId = x.UserId,
                    UserName = x.User?.UserName,
                    AccountNumber = x.BankAccount,
                    Comment = x.Comment,
                    Status = x.Status,
                    Amount = x.Amount,
                    TransactionId= x.TransactionId,
                }),
                Total = await _unitOfWork.TransactionHistoryRepository.CountByFilterAsync()
            });
        }

        [HttpPatch]
        [Route("ProcessPendingTransacion")]
        [Authorize(Roles = UserRolesConstant.Admin)]
        public async Task<IActionResult> ProcessPendingTransacion(RequestTransactionModel model)
        {
            var transaction = await _unitOfWork.TransactionHistoryRepository.GetByIDAsync(model.Id);

            if (transaction != null)
            {
                var user = await GetCurrentUser();

                if (user != null)
                {
                    transaction.Status = EnumTransactionStatus.SUCCESS;

                    _unitOfWork.TransactionHistoryRepository.Update(transaction);

                    user.Balance += transaction.Amount;
                    user.TotalDeposited += transaction.Amount;

                    _unitOfWork.UserRepository.Update(user);

                    // Update wallet history
                    var bank = await _unitOfWork.BankAccountRepository.GetByIDAsync(transaction.BankId);
                    var bankName = bank != null ? bank.BankName : transaction.BankType;
                    await _unitOfWork.WalletHistoryRepository.InsertAsync(new WalletHistoryEntity
                    {
                        UserId = user.Id,
                        Value = transaction.Amount,
                        CreatedDate = DateTime.UtcNow,
                        Note = $"Nap tien qua {bankName}"
                    });

                    await _unitOfWork.SaveChangesAsync();

                    return Ok(new ResponseModel
                    {
                        Status = "Success",
                        Data = new TransactionHistoryResponseModel
                        {
                            Id = transaction.Id,
                            TransactionDate = transaction.TransactionDate,
                            UserId = transaction.UserId,
                            UserName = transaction.User?.UserName,
                            AccountNumber = transaction.BankAccount,
                            Comment = transaction.Comment,
                            Status = transaction.Status,
                            Amount = transaction.Amount,
                            TransactionId = transaction.TransactionId,
                        }
                    });
                }

                return BadRequest("User not found");
            }

            return BadRequest("Transaction not found");
        }
    }
}
