using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class TakeNoteProxyKeyModel
    {
        [Required(ErrorMessage = "Key is required")]
        public string Key { get; set; }
        public string Note { get; set; }
    }
}
