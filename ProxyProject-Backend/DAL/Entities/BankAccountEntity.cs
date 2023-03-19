using ProxyProject_Backend.DAL.Entities.Interface;

namespace ProxyProject_Backend.DAL.Entities
{
    public class BankAccountEntity : CoreEntity
    {
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string ApiLink { get; set; }
        public string Password { get;set; }
        public string BankLogo { get; set; }
        public string Token { get; set; }
        public bool IsMaintainance { get; set; }
    }
}
