using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProxyProject_Backend.DAL;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Models.RequestModels;
using ProxyProject_Backend.Models.Response;
using ProxyProject_Backend.Utils;

namespace ProxyProject_Backend.Controllers
{
    [Route("api/bank-account")]
    [ApiController]
    [Authorize]
    public class BankAccountController : ApiBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BankAccountController(
            ApplicationDbContext context,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment,
            UserManager<UserEntity> userManager
            ) : base(context, userManager)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route("GetBankAccounts")]
        public async Task<IActionResult> GetBankAccounts([FromQuery] GetListPagingModel model)
        {
            var backAccounts = await _unitOfWork.BankAccountRepository
                     .GetAsync(null, null, "", model.PageIndex, model.PageSize);

            return Ok(new ResponseModel
            {
                Status = "Success",
                Data = backAccounts.Select(x => new BankAccountModel
                {
                    Id = x.Id,
                    BankName = x.BankName,
                    BankLogo = x.BankLogo,
                    AccountName = x.AccountName,
                    AccountNumber = x.AccountNumber,
                    IsMaintainance = x.IsMaintainance
                }),
                Total = await _unitOfWork.BankAccountRepository.CountByFilterAsync()
            });
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("AddBankAccount")]
        public async Task<IActionResult> AddBankAccount([FromForm] AddBankAccountModel model)
        {
            if (model.BankLogo != null && model.BankLogo.Length > 0)
            {
                if (FileUtils.IsFileExtensionAllowed(model.BankLogo.FileName, _configuration["AllowedFileExtensions"].Split(",")))
                {
                    var bankAccount = await _unitOfWork.BankAccountRepository.InsertAsync(new BankAccountEntity
                    {
                        BankName = model.BankName,
                        BankLogo  = UploadBankLogo(model.BankName, model.BankLogo),
                        AccountName = model.AccountName,
                        AccountNumber = model.AccountNumber,
                        IsMaintainance = false
                    });

                    await _unitOfWork.SaveChangesAsync();

                    if (bankAccount != null)
                    {
                        return Ok(new ResponseModel
                        {
                            Status = "Success",
                            Data = new BankAccountModel
                            {
                                Id = bankAccount.Id,
                                BankName = bankAccount.BankName,
                                BankLogo = bankAccount.BankLogo,
                                AccountName = bankAccount.AccountName,
                                AccountNumber = bankAccount.AccountNumber,
                                IsMaintainance = bankAccount.IsMaintainance
                            }
                        });
                    }

                    return BadRequest("Error when adding bankAccount");
                }

                return BadRequest("File extention is not allowed");
            }

            return BadRequest("File is empty");
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("UpdateBankAccount")]
        public async Task<IActionResult> UpdateBankAccount([FromForm] UpdateBankAccountModel model)
        {
            var bankAccount = await _unitOfWork.BankAccountRepository.GetByIDAsync(model.Id);

            if (bankAccount != null)
            {
                if (model.BankLogo != null && model.BankLogo.Length > 0)
                {
                    if (!FileUtils.IsFileExtensionAllowed(model.BankLogo.FileName, _configuration["AllowedFileExtensions"].Split(",")))
                    {
                        return BadRequest("File extention is not allowed");
                    }

                    bankAccount.BankLogo = UploadBankLogo(model.BankName, model.BankLogo, bankAccount.BankLogo);
                }

                bankAccount.BankName = model.BankName;
                bankAccount.AccountName = model.AccountName;
                bankAccount.AccountNumber = model.AccountNumber;

                _unitOfWork.BankAccountRepository.Update(bankAccount);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new BankAccountModel
                    {
                        Id = bankAccount.Id,
                        BankName = bankAccount.BankName,
                        BankLogo = bankAccount.BankLogo,
                        AccountName = bankAccount.AccountName,
                        AccountNumber = bankAccount.AccountNumber,
                        IsMaintainance = bankAccount.IsMaintainance
                    }
                });
            }

            return BadRequest("Bank account not found");
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("ToggleBankAccountMaintainance")]
        public async Task<IActionResult> ToggleBankAccountMaintainance(ToggleBankAccountMaintainanceModel model)
        {
            var bankAccount = await _unitOfWork.BankAccountRepository.GetByIDAsync(model.Id);

            if (bankAccount != null)
            {
                bankAccount.IsMaintainance = model.IsMaintainance;
                _unitOfWork.BankAccountRepository.Update(bankAccount);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Data = new BankAccountModel
                    {
                        Id = bankAccount.Id,
                        BankName = bankAccount.BankName,
                        BankLogo = bankAccount.BankLogo,
                        AccountName = bankAccount.AccountName,
                        AccountNumber = bankAccount.AccountNumber,
                        IsMaintainance = bankAccount.IsMaintainance
                    }
                });
            }

            return BadRequest("Bank account not found");
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("DeleteBankAccount")]
        public async Task<IActionResult> DeleteBankAccount(DeleteBankAccountModel model)
        {
            var bankAccount = await _unitOfWork.BankAccountRepository.GetByIDAsync(model.Id);

            if (bankAccount != null)
            {
                _unitOfWork.BankAccountRepository.Delete(bankAccount);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Message = "Delete bank account successfully!"
                });
            }

            return BadRequest("Bank account not found");
        }

        #region private functions
        private string UploadBankLogo(string bankName, IFormFile bankLogo, string oldBankLogo = "")
        {
            var logoPath = string.Empty;

            try
            {
                if(!string.IsNullOrEmpty(oldBankLogo))
                {
                    var fileToDelete = Path.Combine(_hostingEnvironment.WebRootPath, oldBankLogo);

                    if (System.IO.File.Exists(fileToDelete))
                    {
                        System.IO.File.Delete(fileToDelete);
                    }
                }

                // Save the file to the server
                logoPath = $"BankLogo/{bankName}{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds()}{Path.GetExtension(bankLogo.FileName).ToLowerInvariant()}";
                var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, logoPath);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    bankLogo.CopyTo(stream);
                }
            }
            catch
            {
                //Swallow
            }

            return logoPath;
        }
        #endregion
    }
}
