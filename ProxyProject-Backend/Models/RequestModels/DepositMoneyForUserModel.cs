using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class DepositMoneyForUserModel
    {
        [Required(ErrorMessage = "User id is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }
}