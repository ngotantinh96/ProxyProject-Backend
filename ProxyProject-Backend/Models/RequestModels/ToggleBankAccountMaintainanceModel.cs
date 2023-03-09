namespace ProxyProject_Backend.Models.RequestModels
{
    public class ToggleBankAccountMaintainanceModel : RequestBankAccountModel
    {
        public bool IsMaintainance { get; set; }
    }
}
