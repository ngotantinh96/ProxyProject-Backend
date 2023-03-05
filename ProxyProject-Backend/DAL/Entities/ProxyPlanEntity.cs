using ProxyProject_Backend.DAL.Entities.Interface;

namespace ProxyProject_Backend.DAL.Entities
{
    public class ProxyKeyPlansEntity : CoreEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PriceUnit { get; set; }
        public string Description { get; set; }

        public virtual List<ProxyKeysEntity> ProxyKeys { get; set; }
    }
}
