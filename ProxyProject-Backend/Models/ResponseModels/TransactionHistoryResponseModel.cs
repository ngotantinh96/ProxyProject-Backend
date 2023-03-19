using ProxyProject_Backend.DAL.Entities;

namespace ProxyProject_Backend.Models.ResponseModels
{
    public class TransactionHistoryResponseModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string AccountNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Comment { get; set; }
        public EnumTransactionStatus Status { get; set; }
        public decimal Amount { get;set; }
        public string TransactionId { get; set; }
    }
}
