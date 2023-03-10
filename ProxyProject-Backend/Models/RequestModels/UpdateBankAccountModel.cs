using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class UpdateBankAccountModel
    {
        [Required(ErrorMessage = "Bank account id is required")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Bank name is required")]
        public string BankName { get; set; }
        [Required(ErrorMessage = "Bank account name is required")]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "Bank account number is required")]
        public string AccountNumber { get; set; }
        public IFormFile BankLogo { get; set; }

        [Required(ErrorMessage = "IsMaintainance is required")]
        public bool IsMaintainance { get; set; }
    }
}
