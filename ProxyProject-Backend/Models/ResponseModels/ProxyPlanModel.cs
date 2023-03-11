namespace ProxyProject_Backend.Models.Response
{
    public class ProxyPlanModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string PriceUnit { get; set; }
        public string Description { get; set; }
    }
}
