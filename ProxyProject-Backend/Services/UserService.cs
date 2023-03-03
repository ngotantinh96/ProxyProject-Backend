using ProxyProject_Backend.DAL;
using ProxyProject_Backend.Services.Interface;
using ProxyProject_Backend.Utils;

namespace ProxyProject_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private UnitOfWork _unitOfWork;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
            _unitOfWork = new UnitOfWork(context);
        }

        public async Task<string> GenerateUserAPIKeyAsync()
        {
            var apiKey = StringUtils.GenerateSecureKey();

            while (await _unitOfWork.UserRepository.CountByFilterAsync(x => x.APIKey == apiKey) > 0)
            {
                apiKey = StringUtils.GenerateSecureKey();
            }

            return apiKey;
        }

        public async Task<string> GenerateUserWalletKeyAsync()
        {
            var walletKey = StringUtils.GenerateSecureKey();

            while (await _unitOfWork.UserRepository.CountByFilterAsync(x => x.WalletKey == walletKey) > 0)
            {
                walletKey = StringUtils.GenerateSecureKey();
            }

            return walletKey;
        }
    }
}
