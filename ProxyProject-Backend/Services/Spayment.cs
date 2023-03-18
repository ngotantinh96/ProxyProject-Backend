using Newtonsoft.Json;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.Services.Interface;

namespace ProxyProject_Backend.Services
{
    public class Spayment: ISpayment
    {
        private readonly IServiceCollection Services;
        private readonly IServiceProvider serviceProvider;
        public Spayment(ApplicationDbContext context)
        {
           
        }
        public void GenerateMerchandise()
        {
            Console.WriteLine($"Generate Merchandise: long running service at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public void SendEmail()
        {
            Console.WriteLine($"Send Email: delayed execution service at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public async Task SyncRecords()
        {
            await CheckBank();
            Console.WriteLine($"Sync Records: at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public void UpdateDatabase()
        {
            Console.WriteLine($"Update Database: at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        private async Task CheckBank()
        {
            string endpoint = "https://api.spayment.vn/historyapimomo/";
            string token = "ABE98244-9B0D-AB34-A849-84961E1C1162";
            string userName = "";
            string password = "";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(endpoint + token);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    MomoType momoObj = JsonConvert.DeserializeObject<MomoType>(json);
                }
            }

        }
    }

    public class TranList
    {
        public string user { get; set; }
        public long tranId { get; set; }
        public long clientTime { get; set; }
        public int tranType { get; set; }
        public int io { get; set; }
        public string partnerId { get; set; }
        public string partnerName { get; set; }
        public string comment { get; set; }
        public int amount { get; set; }
        public int status { get; set; }
        public int moneySource { get; set; }
        public string desc { get; set; }
        public string serviceId { get; set; }
        public int receiverType { get; set; }
        public string extra { get; set; }
        public string channel { get; set; }
        public string otpType { get; set; }
        public string ipAddress { get; set; }
        public string _class { get; set; }
    }

    public class MomoMsg
    {
        public long begin { get; set; }
        public long end { get; set; }
        public List<TranList> tranList { get; set; }
        public string _class { get; set; }
    }

    public class Extra
    {
        public string originalClass { get; set; }
        public string originalPhone { get; set; }
        public string checkSum { get; set; }
    }

    public class MomoType
    {
        public MomoMsg momoMsg { get; set; }
        public long time { get; set; }
        public string user { get; set; }
        public string lang { get; set; }
        public string msgType { get; set; }
        public bool result { get; set; }
        public string appCode { get; set; }
        public string appVer { get; set; }
        public string channel { get; set; }
        public string deviceOS { get; set; }
        public object ip { get; set; }
        public object localAddress { get; set; }
        public string session { get; set; }
        public Extra extra { get; set; }
    }
}
