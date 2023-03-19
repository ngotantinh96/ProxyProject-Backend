using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class AddBankAccountModel
    {
        [Required(ErrorMessage = "Bank name is required")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "Bank account name is required")]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "Bank account number is required")]
        public string AccountNumber { get; set; }
        public IFormFile BankLogo { get; set; }
        public bool IsMaintainance { get; set; }
        public string Password { get; set; }
    }
}
