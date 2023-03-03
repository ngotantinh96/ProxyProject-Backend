namespace ProxyProject_Backend.Models.Response
{
    public class ProxyKeysModel
    {
        public Guid Id { get; set; }
        public string ProxyKeyPlan { get; set; }
        public string ProxyKey { get; set; }
        public DateTime ExpireDate { get; set; }
        public EnumStatusKey Status { get; set; }
        public string Note { get; set; }
    }

    public enum EnumStatusKey
    {
        EXPIRED = 1,
        WORKING = 0
    }
}
