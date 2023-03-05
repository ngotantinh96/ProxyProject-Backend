namespace ProxyProject_Backend.Models.Response
{
    public class WalletHistoryModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Value { get; set; }
        public string Note { get; set; }
    }
}
