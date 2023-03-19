using ProxyProject_Backend.DAL.Entities.Interface;

namespace ProxyProject_Backend.DAL.Entities
{
    public class TransactionHistoryEntity :CoreEntity
    {
        public string Name { get; set; }
        public string TransactionId { get; set; }
        public Guid BankId { get; set; }
        public string BankAccount { get; set; }
        public string CreatedBy { get; set; }
        public string Comment { get; set; }
        public  DateTime TransactionDate { get; set; }
        public EnumTransactionStatus Status { get; set; }
        public decimal Amount { get; set; }

        public string UserId { get; set; }
        public UserEntity User { get; set; }
    }

    public enum EnumTransactionStatus
    {
       SUCCESS = 1,
       FAIL = 0

    }
}
