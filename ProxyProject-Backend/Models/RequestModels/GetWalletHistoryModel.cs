namespace ProxyProject_Backend.Models.RequestModels
{
    public class GetWalletHistoryModel
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
