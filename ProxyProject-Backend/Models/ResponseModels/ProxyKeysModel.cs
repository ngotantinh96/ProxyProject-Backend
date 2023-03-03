namespace ProxyProject_Backend.Models.Response
{
    public class ProxyKeysModel
    {
        public Guid Id { get; set; }
        public string ProxyKeyPlan { get; set; }
        public string ProxyKey { get; set; }
        public DateTime ExpireDate { get; set; }
        public Enum_StatusKey Status { get; set; }
        public string Note { get; set; }
    }

    public enum Enum_StatusKey
    {
        EXPIRED = 1,
        WORKING = 0
    }
}
