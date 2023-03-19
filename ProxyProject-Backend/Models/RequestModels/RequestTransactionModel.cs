using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class ProcessPendingTransacionModel
    {
        [Required(ErrorMessage = "Transaction id is required")]
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
    }
}