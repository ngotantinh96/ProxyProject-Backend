using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class DeleteNotificationModel
    {
        [Required(ErrorMessage = "Notification Ids is required")]
        public List<Guid> Ids { get; set; }
    }
}
