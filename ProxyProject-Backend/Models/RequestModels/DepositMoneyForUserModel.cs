using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class DepositMoneyForUserModel
    {
        [Required(ErrorMessage = "User id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
    }
}