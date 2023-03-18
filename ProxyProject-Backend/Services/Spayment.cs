using Newtonsoft.Json;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.Models.ResponseModels;
using ProxyProject_Backend.Services.Interface;

namespace ProxyProject_Backend.Services
{
    public class Spayment: ISpayment
    {
        private readonly IConfiguration _configuration;
        private UnitOfWork _unitOfWork;
        public Spayment(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _unitOfWork = new UnitOfWork(context);
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
            var bankAccount = _configuration.GetSection("BankAccounts");
            var bankAccountDb = await _unitOfWork.BankAccountRepository.GetAsync(x => !x.IsMaintainance);

            MomoType momoObj = null;
            using (var httpClient = new HttpClient())
            {
                bankAccountDb.ForEach(x =>
                {
                    string endpoint = string.IsNullOrWhiteSpace(x.ApiLink) ? x.ApiLink :  "https://api.spayment.vn/historyapimomo/";
                    string token = string.IsNullOrWhiteSpace(x.Token) ? x.Token: "ABE98244-9B0D-AB34-A849-84961E1C1162";
                    string userName = string.IsNullOrWhiteSpace(x.AccountName) ? x.AccountName : string.Empty;
                    string password = "";

                    new Task(async () =>
                    {
                        var response = await httpClient.GetAsync(endpoint + token);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            momoObj = JsonConvert.DeserializeObject<MomoType>(json);

                            // TODO : 
                            // Check comment in json
                            // If
                        }
                    });
                });
            }
        }
    }
}
