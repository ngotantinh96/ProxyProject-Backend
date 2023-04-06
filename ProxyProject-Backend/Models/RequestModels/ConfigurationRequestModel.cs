namespace ProxyProject_Backend.Models.RequestModels
{
    public class ConfigurationRequestModel
    {
        public bool TwoFactAuthen { get; set; }
        public int LimitPage { get; set; }
        public int ProxyChangeTime { get; set; }
    }
}
