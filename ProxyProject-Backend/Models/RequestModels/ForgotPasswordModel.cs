using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
