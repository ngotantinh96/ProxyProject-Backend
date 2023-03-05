using ProxyProject_Backend.DAL.Entities.Interface;

namespace ProxyProject_Backend.DAL.Entities
{
    public class NotificationEntity : CoreEntity
    {
        public DateTime CreatedDate { get; set; }
        public EnumNotificationType NotificationType { get; set; }
        public string Message { get; set; }
    }

    public enum EnumNotificationType
    {
        Info = 0,
        Warning = 1,
        Important = 2
    }
}
