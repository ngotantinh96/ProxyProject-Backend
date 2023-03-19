using ProxyProject_Backend.DAL.Entities.Interface;

namespace ProxyProject_Backend.DAL.Entities
{
    public class ProxyHistoryEntity : CoreEntity
    {
        public Guid ProxyId { get; set; }
        public DateTime UsedTime { get; set; }
        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
