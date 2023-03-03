namespace ProxyProject_Backend.Services.Interface
{
    public interface IProxyKeyService
    {
        Task<string> GenerateProxyKeyAsync();
    }
}
