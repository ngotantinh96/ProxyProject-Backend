using Newtonsoft.Json;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.ResponseModels;
using ProxyProject_Backend.Services.Interface;
using System.Globalization;

namespace ProxyProject_Backend.Services
{
    public class Spayment : ISpayment
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

        public void SyncRecords()
        {
            CheckBank();
            Console.WriteLine($"Sync Records: at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public void UpdateDatabase()
        {
            Console.WriteLine($"Update Database: at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        private void CheckBank()
        {
            var bankAccountDb = _unitOfWork.BankAccountRepository.GetAsync(x => !x.IsMaintainance).GetAwaiter().GetResult();
            bankAccountDb.ForEach((bank) =>
            {
                var transactionHistory = _unitOfWork.TransactionHistoryRepository
                .GetAsync(p => p.BankId == bank.Id, p => p.OrderByDescending(t => t.TransactionDate), "", 0, 1).GetAwaiter().GetResult();
                switch (bank.BankName.ToUpper())
                {
                    case "MOMO":
                        GetMomoBank(bank, transactionHistory);
                        break;
                    case "VIETCOMBANK":
                        GetVCBBank(bank, transactionHistory);
                        break;
                    case "ACBBANK":
                        GetACBBank(bank, transactionHistory);
                        break;
                }
            });
        }

        private void GetMomoBank(BankAccountEntity bank, List<TransactionHistoryEntity> transactionHistory)
        {
            string endpoint = !string.IsNullOrWhiteSpace(bank.ApiLink) ? bank.ApiLink : "https://api.spayment.vn/historyapimomo/token";
            string token = !string.IsNullOrWhiteSpace(bank.Token) ? bank.Token : "ABE98244-9B0D-AB34-A849-84961E1C1162";
            string userName = !string.IsNullOrWhiteSpace(bank.AccountName) ? bank.AccountName : string.Empty;
            endpoint = endpoint.Replace("token", token);

            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(endpoint + token).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    if (string.IsNullOrEmpty(json))
                    {
                        response = httpClient.GetAsync(endpoint + token).Result;
                        json = response.Content.ReadAsStringAsync().Result;
                    }
                    MomoType momoObj = JsonConvert.DeserializeObject<MomoType>(json);

                    if (momoObj?.momoMsg != null && momoObj.momoMsg.tranList.Any())
                    {
                        var transactionList = new List<TranList>();
                        if (transactionHistory == null || transactionHistory.Count == 0)
                        {
                            transactionList.Add(momoObj.momoMsg.tranList.FirstOrDefault());
                        }
                        else
                        {
                            var transactionApi = momoObj.momoMsg.tranList.Select(x => new
                            {
                                TransactionDate = new DateTime(1970, 1, 1, 0, 0, 0, 0)
                                .AddMilliseconds(x.clientTime ?? 0),
                                TransactionId = x.tranId.ToString(),
                            }).ToList();

                            var lastTransaction = transactionHistory.OrderByDescending(x => x.TransactionDate).FirstOrDefault();

                            var test = transactionApi
                                .Where(x => !x.TransactionId.Equals(lastTransaction.TransactionId) && x.TransactionDate >= lastTransaction.TransactionDate).ToList();

                            transactionList = momoObj.momoMsg.tranList.Where(_ => new DateTime(1970, 1, 1, 0, 0, 0, 0)
                            .AddMilliseconds(_.clientTime ?? 0) >= transactionHistory
                            .FirstOrDefault().TransactionDate
                            && !_.tranId.ToString().Equals(transactionHistory
                            .FirstOrDefault().TransactionId)).ToList();
                        }

                        // Save DB
                        transactionList.ForEach(tran =>
                        {
                            if(_unitOfWork.TransactionHistoryRepository.GetByFilterAsync(x => x.TransactionId == tran.tranId.ToString()).Result == null)
                            {
                                if (!string.IsNullOrWhiteSpace(tran.comment) && Guid.TryParse(tran.comment?.Trim(), out Guid userId))
                                {
                                    var user = _unitOfWork.UserRepository.GetByFilterAsync(x => x.Id == userId.ToString()).Result;

                                    _unitOfWork.TransactionHistoryRepository.InsertAsync(new TransactionHistoryEntity()
                                    {
                                        TransactionId = tran.tranId.ToString(),
                                        Name = tran.partnerName,
                                        BankAccount = tran.partnerId,
                                        Amount = tran.amount ?? 0,
                                        BankId = bank.Id,
                                        BankType ="MOMO",
                                        TransactionDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(tran.clientTime ?? 0),
                                        Comment = tran.comment,
                                        Status = user == null ? EnumTransactionStatus.FAIL : EnumTransactionStatus.SUCCESS,
                                        UserId = user.Id,
                                    }).GetAwaiter();

                                    /// Add Price
                                    if (user != null)
                                    {
                                        user.Balance += tran.amount ?? 0;
                                        user.TotalDeposited += tran.amount ?? 0;

                                        _unitOfWork.UserRepository.Update(user);

                                        // Update wallet history
                                        _unitOfWork.WalletHistoryRepository.InsertAsync(new WalletHistoryEntity
                                        {
                                            UserId = user.Id,
                                            Value = tran.amount ?? 0,
                                            CreatedDate = DateTime.UtcNow,
                                            Note = $"Nap tien qua {bank.BankName}"
                                        }).GetAwaiter();
                                    }

                                    _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                                }
                                else
                                {
                                    _unitOfWork.TransactionHistoryRepository.InsertAsync(new TransactionHistoryEntity()
                                    {
                                        TransactionId = tran.tranId.ToString(),
                                        Name = tran.partnerName,
                                        BankAccount = tran.partnerId,
                                        BankId = bank.Id,
                                        BankType = "MOMO",
                                        Amount = tran.amount ?? 0,
                                        TransactionDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(tran.clientTime ?? 0),
                                        Comment = tran.comment,
                                        Status = EnumTransactionStatus.FAIL,
                                    }).GetAwaiter();

                                    _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                                }
                            }
                        });
                    }
                }
            }

        }
        private void GetVCBBank(BankAccountEntity bank, List<TransactionHistoryEntity> transactionHistory)
        {
            string endpoint = !string.IsNullOrWhiteSpace(bank.ApiLink) ? bank.ApiLink : "https://api.spayment.vn/historyapivcb/password/sotaikhoan/token";
            string token = !string.IsNullOrWhiteSpace(bank.Token) ? bank.Token : "39D6670A-1B9A-A12B-ADB0-DB020B35F5CF";
            string userName = !string.IsNullOrWhiteSpace(bank.AccountName) ? bank.AccountName : string.Empty;
            string bankNumber = !string.IsNullOrWhiteSpace(bank.AccountNumber) ? bank.AccountNumber : "123456789123";
            string password = bank.Password;
            endpoint = endpoint.Replace("password", password).Replace("token", token).Replace("sotaikhoan", bankNumber);

            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(endpoint).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    RootObject vcbObj = JsonConvert.DeserializeObject<RootObject>(json);

                    if (vcbObj.data != null && vcbObj.data.ChiTietGiaoDich.Any())
                    {
                        var transactionList = new List<ChiTietGiaoDich>();
                        if (transactionHistory == null || transactionHistory.Count == 0)
                        {
                            transactionList.Add(vcbObj.data.ChiTietGiaoDich.FirstOrDefault());
                        }
                        else
                        {
                            transactionList = vcbObj.data.ChiTietGiaoDich
                            .Where(_ => DateTime.ParseExact(_.NgayGiaoDich, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture) >= transactionHistory
                            .FirstOrDefault().TransactionDate
                            && _.CD == "+"
                            && _.SoThamChieu.ToString() != transactionHistory
                            .FirstOrDefault().TransactionId).ToList();
                        }

                        // Save DB
                        transactionList.ForEach(async tran =>
                        {
                            if (!string.IsNullOrWhiteSpace(tran.MoTa) && Guid.TryParse(tran.MoTa?.Trim(), out Guid userId))
                            {
                                var user = _unitOfWork.UserRepository.GetByFilterAsync(x => x.Id == userId.ToString()).Result;

                                _unitOfWork.TransactionHistoryRepository.InsertAsync(new TransactionHistoryEntity()
                                {
                                    TransactionId = tran.SoThamChieu,
                                    Name = tran.SoThamChieu,
                                    BankAccount = tran.SoThamChieu,
                                    BankId = bank.Id,
                                    BankType = "VIETCOMBANK",
                                    TransactionDate = DateTime.ParseExact(tran.NgayGiaoDich, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                    Comment = tran.MoTa,
                                    Status = user == null ? EnumTransactionStatus.FAIL : EnumTransactionStatus.SUCCESS,
                                    UserId = user.Id,
                                }).GetAwaiter();

                                /// Add Price
                                if (user != null)
                                {
                                    decimal amout = Decimal.Parse(tran.SoTienGhiCo.Replace(",", ""));
                                    user.Balance += amout;
                                    user.TotalDeposited += amout;

                                    _unitOfWork.UserRepository.Update(user);

                                    // Update wallet history
                                    _unitOfWork.WalletHistoryRepository.InsertAsync(new WalletHistoryEntity
                                    {
                                        UserId = user.Id,
                                        Value = amout,
                                        CreatedDate = DateTime.UtcNow,
                                        Note = $"Nap tien qua {bank.BankName}"
                                    }).GetAwaiter();
                                }

                                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                            }
                            else
                            {
                                _unitOfWork.TransactionHistoryRepository.InsertAsync(new TransactionHistoryEntity()
                                {
                                    TransactionId = tran.SoThamChieu,
                                    Name = tran.SoThamChieu,
                                    BankAccount = tran.SoThamChieu,
                                    BankType = "VIETCOMBANK",
                                    BankId = bank.Id,
                                    TransactionDate = DateTime.ParseExact(tran.NgayGiaoDich, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                                    Comment = tran.MoTa,
                                    Status = EnumTransactionStatus.FAIL,
                                }).GetAwaiter();

                                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                            }
                        });
                    }
                }
            }
        }
        private void GetACBBank(BankAccountEntity bank, List<TransactionHistoryEntity> transactionHistory)
        {
            string endpoint = !string.IsNullOrWhiteSpace(bank.ApiLink) ? bank.ApiLink : "https://api.spayment.vn/historyapiacb/password/sotaikhoan/token";
            string token = !string.IsNullOrWhiteSpace(bank.Token) ? bank.Token : "39D6670A-1B9A-A12B-ADB0-DB020B35F5CF";
            string userName = !string.IsNullOrWhiteSpace(bank.AccountName) ? bank.AccountName : string.Empty;
            string bankNumber = !string.IsNullOrWhiteSpace(bank.AccountNumber) ? bank.AccountNumber : "123456789123";
            string password = bank.Password;
            endpoint = endpoint.Replace("password", password).Replace("token", token).Replace("sotaikhoan", bankNumber);

            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(endpoint).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    ACBType acbObject = JsonConvert.DeserializeObject<ACBType>(json);

                    if (acbObject.transactions != null && acbObject.transactions.Any())
                    {
                        var transactionList = new List<Transaction>();
                        if (transactionHistory == null || transactionHistory.Count == 0)
                        {
                            transactionList.Add(acbObject.transactions.FirstOrDefault());
                        }
                        else
                        {
                            transactionList = acbObject.transactions
                            .Where(_ => new DateTime(1970, 1, 1, 0, 0, 0, 0)
                            .AddMilliseconds(_.activeDatetime) >= transactionHistory
                            .FirstOrDefault().TransactionDate
                            && !_.transactionNumber.ToString().Equals(transactionHistory
                            .FirstOrDefault().TransactionId)).ToList();
                        }

                        // Save DB
                        transactionList.ForEach(async tran =>
                        {
                            if (!string.IsNullOrWhiteSpace(tran.description) && Guid.TryParse(tran.description?.Trim(), out Guid userId))
                            {
                                var user = _unitOfWork.UserRepository.GetByFilterAsync(x => x.Id == userId.ToString()).Result;

                                _unitOfWork.TransactionHistoryRepository.InsertAsync(new TransactionHistoryEntity()
                                {
                                    TransactionId = tran.transactionNumber.ToString(),
                                    Name = tran.transactionNumber.ToString(),
                                    BankAccount = tran.transactionNumber.ToString(),
                                    BankId = bank.Id,
                                    BankType = "ACBBANK",
                                    TransactionDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(tran.activeDatetime),
                                    Comment = tran.description,
                                    Status = user == null ? EnumTransactionStatus.FAIL : EnumTransactionStatus.SUCCESS,
                                    UserId = user.Id,
                                }).GetAwaiter();

                                /// Add Price
                                if (user != null)
                                {
                                    decimal amout = Decimal.Parse(tran.amount.ToString());
                                    user.Balance += amout;
                                    user.TotalDeposited += amout;

                                    _unitOfWork.UserRepository.Update(user);

                                    // Update wallet history
                                    _unitOfWork.WalletHistoryRepository.InsertAsync(new WalletHistoryEntity
                                    {
                                        UserId = user.Id,
                                        Value = amout,
                                        CreatedDate = DateTime.UtcNow,
                                        Note = $"Nap tien qua {bank.BankName}"
                                    }).GetAwaiter();
                                }

                                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                            }
                            else
                            {
                                _unitOfWork.TransactionHistoryRepository.InsertAsync(new TransactionHistoryEntity()
                                {
                                    TransactionId = tran.transactionNumber.ToString(),
                                    Name = tran.transactionNumber.ToString(),
                                    BankAccount = tran.transactionNumber.ToString(),
                                    BankId = bank.Id,
                                    BankType = "ACBBANK",
                                    TransactionDate = new DateTime(1970, 1, 1, 0, 0, 0, 0)
                                        .AddMilliseconds(tran.activeDatetime),
                                    Comment = tran.description,
                                    Status = EnumTransactionStatus.FAIL
                                }).GetAwaiter();

                                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                            }
                        });
                    }
                }
            }
        }
    }
}
