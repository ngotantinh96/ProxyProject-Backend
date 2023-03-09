namespace ProxyProject_Backend.Models.Response
{
    public class UserInfoModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string APIKey { get; set; }
        public string WalletKey { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalDeposited { get; set; }
        public int LimitKeysToCreate { get; set; }
        public int NoOfCreatedKeys { get; set; }
    }
}
