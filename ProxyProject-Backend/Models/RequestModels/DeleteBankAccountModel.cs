using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class DeleteBankAccountModel
    {
        [Required(ErrorMessage = "Bank Id is required")]
        public Guid Id { get; set; }
    }
}
