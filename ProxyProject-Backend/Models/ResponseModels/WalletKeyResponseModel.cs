namespace ProxyProject_Backend.Models.ResponseModels
{
    public class WalletKeyResponseModel
    {
        public string WalletKey { get; set; }
        public int LimitKeysToCreate { get; set; }
        public decimal TotalDeposited { get; set; }
        public decimal Balance { get; set; }
    }
}
