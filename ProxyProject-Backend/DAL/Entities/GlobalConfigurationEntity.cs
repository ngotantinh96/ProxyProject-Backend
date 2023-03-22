using ProxyProject_Backend.DAL.Entities.Interface;

namespace ProxyProject_Backend.DAL.Entities
{
    public class GlobalConfigurationEntity : CoreEntity
    {
        public bool TwoFactorEnabled { get; set; }         
    }
}
