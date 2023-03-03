using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class OrderProxyKeysModel
    {
        [Required(ErrorMessage = "ProxyKeyPlanId is required")]
        public Guid ProxyKeyPlanId { get; set; }
        [Required(ErrorMessage = "NoOfKeys is required")]
        public int NoOfKeys { get; set; }
        [Required(ErrorMessage = "NoOfDates is required")]
        public int NoOfDates { get; set; }
    }
}
