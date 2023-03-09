using ProxyProject_Backend.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class UpdateNotificationModel : RequestNotificationModel
    {
        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
        [Required(ErrorMessage = "NotificationType is required")]
        public EnumNotificationType NotificationType { get; set; }
    }
}
