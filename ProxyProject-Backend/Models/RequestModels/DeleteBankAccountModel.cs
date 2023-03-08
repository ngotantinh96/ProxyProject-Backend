using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class DeleteBankAccountModel
    {
        [Required(ErrorMessage = "Bank Ids is required")]
        public List<Guid> Ids { get; set; }
    }
}
