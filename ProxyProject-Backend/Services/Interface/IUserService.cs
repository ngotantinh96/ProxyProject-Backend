namespace ProxyProject_Backend.Services.Interface
{
    public interface IUserService
    {
        Task<string> GenerateUserWalletKeyAsync();
        Task<string> GenerateUserAPIKeyAsync();
    }
}
