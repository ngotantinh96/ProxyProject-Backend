using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class ExtendProxyKeysModel
    {
        [Required(ErrorMessage = "Keys is required")]
        public List<string> Keys { get; set; }
        public List<Guid> Ids { get; set; }
        [Required(ErrorMessage = "NoOfDates is required")]
        public int NoOfDates { get; set; }
    }
}
