using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class UpdateProxyPlanModel : RequestProxyPlansModel
    {
        [Required(ErrorMessage = "Plan name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Plan price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Price unit is required")]
        public string PriceUnit { get; set; }
        public string Description { get; set; }
    }
}
