using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class UploadProxyModel
    {
        [Required(ErrorMessage = "Country id is required")]
        public Guid CountryId { get; set; }
        public IFormFile ProxyFile { get; set; }
    }
}