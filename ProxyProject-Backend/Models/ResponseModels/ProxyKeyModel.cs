namespace ProxyProject_Backend.Models.Response
{
    public class ProxyKeyModel
    {
        public Guid Id { get; set; }
        public string ProxyKeyPlan { get; set; }
        public decimal PlanPrice { get; set; }
        public string PlanPriceUnit { get; set; }
        public string ProxyKey { get; set; }
        public DateTime ExpireDate { get; set; }
        public EnumStatusKey Status { get; set; }
        public string Note { get; set; }
        public bool IsInUse { get; set; }
    }

    public enum EnumStatusKey
    {
        EXPIRED = 1,
        WORKING = 0
    }
}
