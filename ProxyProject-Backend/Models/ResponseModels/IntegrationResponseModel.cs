using ProxyProject_Backend.DAL.Entities.Interface;
using ProxyProject_Backend.DAL.Entities;
using System.Text.Json.Serialization;

namespace ProxyProject_Backend.Models.Response
{
    public class IntegrationCommonResponseModel
    {
        public bool Success { get; set; }
        public string Description { get; set; }
    }

    public class IntegrationResponseModel : IntegrationCommonResponseModel
    {
        public object Data { get; set; }
    }

    public class IntegrationUserInfoModel
    {
        public string WalletKey { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalDeposited { get; set; }
        public int MaxKey { get; set; }
    }

    public class IntegrationProxyModel
    {
        public string Proxy { get; set; }
        public double NextChange { get; set; }
        public double Timeout { get; set; }
        public string Country { get; set; }
    }

    public class IntegrationProxyKeyModel
    {
        public DateTime DateExpired { get; set; }
        public bool IsInUse { get; set; }
    }
}
