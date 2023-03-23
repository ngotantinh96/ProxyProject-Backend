namespace ProxyProject_Backend.Models.Response
{
    public class WalletInfoModel
    {
        public decimal Balance { get; set; }
        public decimal TotalDeposited { get; set; }
        public string BankMemo { get; set; }
        public List<WalletHistoryModel> History { get; set; }         
    }
}
