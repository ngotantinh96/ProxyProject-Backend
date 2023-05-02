using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
using ProxyProject_Backend.Models.Response;
using ProxyProject_Backend.Models.ResponseModels;
using System.Data;
using System.Linq.Expressions;

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
            Func<IQueryable<TransactionHistoryEntity>, IOrderedQueryable<TransactionHistoryEntity>> orderBy = (x) => x.OrderByDescending(p => p.CreatedDate);

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                filter = (x) => x.BankType.Contains(request.Keyword);
            }

            var result = await _unitOfWork.TransactionHistoryRepository.GetAsync(filter, orderBy,"User",request.PageIndex, request.PageSize ?? 10);

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
                    TransactionType = x.BankType
                }),
                Total = await _unitOfWork.TransactionHistoryRepository.CountByFilterAsync()
            });
        }

        [HttpPatch]
        [Route("ProcessPendingTransacion")]
        [Authorize(Roles = UserRolesConstant.Admin)]
        public async Task<IActionResult> ProcessPendingTransacion(ProcessPendingTransacionModel model)
        {
            var transaction = await _unitOfWork.TransactionHistoryRepository.GetByIDAsync(model.Id);

            if (transaction != null)
            {
                var user = await GetCurrentUser();
                var userPayment = await _unitOfWork.UserRepository.GetByFilterAsync(x => x.UserName.Equals(model.UserName));

                if (user != null && userPayment != null)
                {
                    if(model.Amount > 0)
                    {
                        transaction.Amount = model.Amount;
                    }

                    transaction.Status = EnumTransactionStatus.SUCCESS;
                    transaction.CreatedBy = user.Id;
                    transaction.Comment = !string.IsNullOrWhiteSpace(model.Note) ? model.Note : user.Id;
                    _unitOfWork.TransactionHistoryRepository.Update(transaction);

                    userPayment.Balance += transaction.Amount;
                    userPayment.TotalDeposited += transaction.Amount;
                    transaction.UserId = userPayment.Id;
                    _unitOfWork.UserRepository.Update(userPayment);

                    // Update wallet history
                    var bank = await _unitOfWork.BankAccountRepository.GetByIDAsync(transaction.BankId);
                    var bankName = bank != null ? bank.BankName : transaction.BankType;
                    await _unitOfWork.WalletHistoryRepository.InsertAsync(new WalletHistoryEntity(await GetCurrentUser())
                    {
                        UserId = userPayment.Id,
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
