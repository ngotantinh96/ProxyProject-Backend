namespace ProxyProject_Backend.Services.Interface
{
    public interface IEmailService
    {
        Task<bool> SendMailAsync(string subject, string body, string recipient);
    }
}
