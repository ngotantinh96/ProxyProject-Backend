using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class RequestTransactionModel
    {
        [Required(ErrorMessage = "Transaction id is required")]
        public Guid Id { get; set; }
    }
}