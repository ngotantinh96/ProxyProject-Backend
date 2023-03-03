using ProxyProject_Backend.DAL;
using ProxyProject_Backend.Services.Interface;
using ProxyProject_Backend.Utils;

namespace ProxyProject_Backend.Services
{
    public class ProxyKeyService : IProxyKeyService
    {
        private readonly ApplicationDbContext _context;
        private UnitOfWork _unitOfWork;

        public ProxyKeyService(ApplicationDbContext context)
        {
            _context = context;
            _unitOfWork = new UnitOfWork(context);
        }

        public async Task<string> GenerateProxyKeyAsync()
        {
            var proxyKey = StringUtils.GenerateSecureKey();

            while (await _unitOfWork.ProxyKeysRepository.CountByFilterAsync(x => x.Key == proxyKey) > 0)
            {
                proxyKey = StringUtils.GenerateSecureKey();
            }

            return proxyKey;
        }
    }
}
