namespace ProxyProject_Backend.Models.RequestModels
{
    public class GetListPagingModel
    {
        public string Keyword { get; set; }
        public int? IsUse { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
