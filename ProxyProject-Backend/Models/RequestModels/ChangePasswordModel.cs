using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }
    }
}
