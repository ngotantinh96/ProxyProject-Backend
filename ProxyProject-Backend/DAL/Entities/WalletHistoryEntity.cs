using ProxyProject_Backend.DAL.Entities.Interface;

namespace ProxyProject_Backend.DAL.Entities
{
    public class WalletHistoryEntity : CoreEntity
    {
        public DateTime CreatedDate { get; set; }
        public decimal Value { get; set; }
        public string Note { get; set; }

        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
