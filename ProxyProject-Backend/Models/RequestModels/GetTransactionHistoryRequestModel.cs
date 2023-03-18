namespace ProxyProject_Backend.Models.RequestModels
{
    public class GetTransactionHistoryRequestModel
    {
        public string Keyword { get; set;}
        public int Page { get; set; }
        public int? Size { get; set; }
    }
}
