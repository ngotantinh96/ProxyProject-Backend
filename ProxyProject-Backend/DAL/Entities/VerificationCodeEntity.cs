using ProxyProject_Backend.DAL.Entities.Interface;

namespace ProxyProject_Backend.DAL.Entities
{
    public class VerificationCodeEntity : CoreEntity
    {
        public string Code { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Email { get; set; }
    }
}
