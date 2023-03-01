namespace ProxyProject_Backend.DAL.Entities
{
    public class ProxyEntity : CoreEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PriceUnit { get; set; }
        public string Description { get; set; }
    }
}
