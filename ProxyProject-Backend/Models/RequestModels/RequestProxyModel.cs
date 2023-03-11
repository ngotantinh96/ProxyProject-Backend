using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class RequestProxyModel
    {
        [Required(ErrorMessage = "Proxy id is required")]
        public Guid Id { get; set; }
    }
}
