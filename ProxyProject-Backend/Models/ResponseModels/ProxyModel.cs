namespace ProxyProject_Backend.Models.Response
{
    public class ProxyModel
    {
        public Guid Id { get; set; }
        public string Proxy { get; set; }
        public DateTime? StartUsingTime { get; set; }
        public DateTime? EndUsingTime { get; set; }
        public Guid ProxyKeyPlanId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public int IsUse { get;set; }
    }
}
