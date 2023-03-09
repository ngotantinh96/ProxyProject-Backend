using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class RequestUserModel
    {
        [Required(ErrorMessage = "User id is required")]
        public Guid Id { get; set; }
    }
}
