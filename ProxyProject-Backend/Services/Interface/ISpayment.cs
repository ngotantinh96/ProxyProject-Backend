namespace ProxyProject_Backend.Services.Interface
{
    public interface ISpayment
    {
        void SendEmail();
        void UpdateDatabase();
        void GenerateMerchandise();
        Task SyncRecords();
    }
}
