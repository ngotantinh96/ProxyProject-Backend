using ProxyProject_Backend.DAL.Entities;

namespace ProxyProject_Backend.DAL
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private GenericRepository<UserEntity> _userRepository;
        private GenericRepository<ProxyKeyPlansEntity> _proxyKeyPlansRepository;
        private GenericRepository<ProxyKeysEntity> _proxyKeysRepository;
        private GenericRepository<WalletHistoryEntity> _walletHistoryRepository;

        public GenericRepository<UserEntity> UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new GenericRepository<UserEntity>(_context);
                }
                return _userRepository;
            }
        }

        public GenericRepository<ProxyKeyPlansEntity> ProxyKeyPlansRepository
        {
            get
            {
                if (_proxyKeyPlansRepository == null)
                {
                    _proxyKeyPlansRepository = new GenericRepository<ProxyKeyPlansEntity>(_context);
                }
                return _proxyKeyPlansRepository;
            }
        }

        public GenericRepository<ProxyKeysEntity> ProxyKeysRepository
        {
            get
            {
                if (_proxyKeysRepository == null)
                {
                    _proxyKeysRepository = new GenericRepository<ProxyKeysEntity>(_context);
                }
                return _proxyKeysRepository;
            }
        }

        public GenericRepository<WalletHistoryEntity> WalletHistoryRepository
        {
            get
            {
                if (_walletHistoryRepository == null)
                {
                    _walletHistoryRepository = new GenericRepository<WalletHistoryEntity>(_context);
                }
                return _walletHistoryRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
