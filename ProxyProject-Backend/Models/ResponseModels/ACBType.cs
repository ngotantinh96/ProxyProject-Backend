namespace ProxyProject_Backend.Models.ResponseModels
{
    public class Transaction
    {
        public int amount { get; set; }
        public string accountName { get; set; }
        public string receiverName { get; set; }
        public int transactionNumber { get; set; }
        public string description { get; set; }
        public string bankName { get; set; }
        public bool isOnline { get; set; }
        public long postingDate { get; set; }
        public string accountOwner { get; set; }
        public string type { get; set; }
        public string receiverAccountNumber { get; set; }
        public string currency { get; set; }
        public int account { get; set; }
        public long activeDatetime { get; set; }
        public long effectiveDate { get; set; }
    }

    public class ACBType
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int took { get; set; }
        public List<Transaction> transactions { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int size { get; set; }
    }
}
