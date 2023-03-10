using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class RequestProxyPlansModel
    {
        [Required(ErrorMessage = "Proxy plan id is required")]
        public Guid Id { get; set; }
    }
}
