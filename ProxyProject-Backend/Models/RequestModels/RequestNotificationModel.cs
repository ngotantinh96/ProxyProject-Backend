using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class RequestNotificationModel
    {
        [Required(ErrorMessage = "Notification Id is required")]
        public Guid Id { get; set; }
    }
}
