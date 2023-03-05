namespace ProxyProject_Backend.Models.Response
{
    public class BankAccountModel
    {
        public Guid Id { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankLogo { get; set; }
        public bool IsMaintainance { get; set; }
    }
}
