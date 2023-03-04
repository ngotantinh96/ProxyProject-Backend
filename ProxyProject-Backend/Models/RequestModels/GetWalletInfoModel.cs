namespace ProxyProject_Backend.Models.RequestModels
{
    public class GetWalletInfoModel
    {
        public string WalleyKey { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
