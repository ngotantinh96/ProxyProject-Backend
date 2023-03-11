using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class AddProxyModel
    {
        [Required(ErrorMessage = "Country id is required")]
        public Guid CountryId { get; set; }
        [Required(ErrorMessage = "Proxies is required")]
        public List<string> Proxies { get; set; }     
    }
}