using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class ExtendProxyKeysModel
    {
        [Required(ErrorMessage = "Keys are required")]
        public List<string> Keys { get; set; }
        [Required(ErrorMessage = "NoOfDates is required")]
        public int NoOfDates { get; set; }
    }
}
