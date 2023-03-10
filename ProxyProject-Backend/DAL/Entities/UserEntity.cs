using Microsoft.AspNetCore.Identity;

namespace ProxyProject_Backend.DAL.Entities
{
    public class UserEntity : IdentityUser
    {
        public string APIKey { get; set; }
        public string WalletKey { get; set; }
        public int LimitKeysToCreate { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalDeposited { get; set; }

        public virtual List<ProxyKeysEntity> ProxyKeys { get; set; }
        public virtual List<WalletHistoryEntity> WalletHistories { get; set; }
    }

    public static class UserRolesConstant
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
