using ProxyProject_Backend.DAL.Entities.Interface;

namespace ProxyProject_Backend.DAL.Entities
{
    public class GlobalConfigurationEntity : CoreEntity
    {
        public bool TwoFactorEnabled { get; set; }
        public int LimitPage { get; set; } = 5;
        public double ProxyChangeTime { get; set; } = 120;
    }
}
