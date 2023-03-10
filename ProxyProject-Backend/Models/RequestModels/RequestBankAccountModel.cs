using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class RequestBankAccountModel
    {
        [Required(ErrorMessage = "Bank id is required")]
        public Guid Id { get; set; }
    }
}
