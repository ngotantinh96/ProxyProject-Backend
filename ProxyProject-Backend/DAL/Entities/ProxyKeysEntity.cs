using ProxyProject_Backend.DAL.Entities.Interface;

namespace ProxyProject_Backend.DAL.Entities
{
    public class ProxyKeysEntity : CoreEntity
    {
        public string Key { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Note { get; set; }

        public Guid ProxyKeyPlanId { get; set; }
        public virtual ProxyKeyPlansEntity ProxyKeyPlan { get; set; }

        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
