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
                    Comment= x.Comment,
                    Status= x.Status,
                }),
                Total = await _unitOfWork.WalletHistoryRepository.CountByFilterAsync()
            });
        }
    }
}
