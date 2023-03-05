using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class DeleteNotificationModel
    {
        [Required(ErrorMessage = "Notification Id is required")]
        public Guid Id { get; set; }
    }

    public class ToggleBankAccountMaintainanceModel
    {
        [Required(ErrorMessage = "Notification Id is required")]
        public Guid Id { get; set; }
        public bool IsMaintainance { get; set; }
    }
}
