namespace ProxyProject_Backend.Models.RequestModels
{
    public class GetTransactionHistoryRequestModel
    {
        public string Keyword { get; set;}
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
