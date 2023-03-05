using ProxyProject_Backend.DAL.Entities;

namespace ProxyProject_Backend.Models.Response
{
    public class NotificationModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public EnumNotificationType NotificationType { get; set; }
        public string Message { get; set; }
    }
}
