using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class DeleteProxyModel
    {
        [Required(ErrorMessage = "Proxy Ids is required")]
        public List<Guid> Ids { get; set; }
    }
}
