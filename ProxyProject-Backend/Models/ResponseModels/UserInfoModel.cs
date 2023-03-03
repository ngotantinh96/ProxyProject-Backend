using ProxyProject_Backend.DAL.Entities;

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
    }
}
