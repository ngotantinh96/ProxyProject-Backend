using ProxyProject_Backend.Services.Interface;
using System.Net;
using System.Net.Mail;

namespace ProxyProject_Backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendMailAsync(string subject, string body, string recipient)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]))
                {
                    Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Smtp:Username"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(recipient);

                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
