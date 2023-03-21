using System.ComponentModel.DataAnnotations;

namespace ProxyProject_Backend.Models.RequestModels
{
    public class ChangeBankPasswordModel : RequestBankAccountModel
    {
        public string CurentPassword { get; set; }
        public string Password { get; set; }
    }

}
