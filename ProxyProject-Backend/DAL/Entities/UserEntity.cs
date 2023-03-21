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
        public virtual List<TransactionHistoryEntity> TransactionHistories { get; set; }
        public virtual List<ProxyHistoryEntity> ProxyHistories { get; set; }
    }

    public class AdminAccount
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int LimitKeysToCreate { get; set; }
    }

    public static class UserRolesConstant
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
