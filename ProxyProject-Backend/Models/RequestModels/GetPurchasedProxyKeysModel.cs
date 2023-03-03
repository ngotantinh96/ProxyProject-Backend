namespace ProxyProject_Backend.Models.RequestModels
{
    public class GetPurchasedProxyKeysModel
    {
        public int Page { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}
