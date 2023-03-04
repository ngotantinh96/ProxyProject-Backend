using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class DeleteProxyKeysModel
    {
        [Required(ErrorMessage = "Keys are required")]
        public List<string> Keys { get; set; }
    }
}
