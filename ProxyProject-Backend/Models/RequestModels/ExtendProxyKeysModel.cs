using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class ExtendProxyKeysModel
    {
        [Required(ErrorMessage = "Keys is required")]
        public List<Guid> Ids { get; set; }
        public List<ListExtendKey> Keys { get; set; }
    }

    public class ListExtendKey
    {
        [Required(ErrorMessage = "Keys is required")]
        public string Key { get; set; }
        public int NoOfDate { get; set; }
    }
}
