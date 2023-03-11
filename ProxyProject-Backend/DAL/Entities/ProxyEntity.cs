using ProxyProject_Backend.DAL.Entities.Interface;

namespace ProxyProject_Backend.DAL.Entities
{
    public class ProxyEntity : CoreEntity
    {
        public string Proxy { get; set; }
        public DateTime? StartUsingTime { get; set; }
        public DateTime? EndUsingTime { get; set; }

        public Guid ProxyKeyPlanId { get; set; }
        public virtual ProxyKeyPlansEntity ProxyKeyPlan { get; set; }
    }
}
