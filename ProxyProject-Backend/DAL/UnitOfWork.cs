using ProxyProject_Backend.DAL.Entities;

namespace ProxyProject_Backend.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private GenericRepository<UserEntity> _userRepository;
        private GenericRepository<ProxyKeyPlansEntity> _proxyKeyPlansRepository;
        private GenericRepository<ProxyKeysEntity> _proxyKeysRepository;
        private GenericRepository<WalletHistoryEntity> _walletHistoryRepository;
        private GenericRepository<BankAccountEntity> _bankAccountRepository;
        private GenericRepository<NotificationEntity> _notificationRepository;
        private GenericRepository<ProxyEntity> _proxyRepository;
        private GenericRepository<TransactionHistoryEntity> _transactionHistoryRepository;

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

        public GenericRepository<ProxyEntity> ProxyRepository
        {
            get
            {
                if (_proxyRepository == null)
                {
                    _proxyRepository = new GenericRepository<ProxyEntity>(_context);
                }
                return _proxyRepository;
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

        public GenericRepository<BankAccountEntity> BankAccountRepository
        {
            get
            {
                if (_bankAccountRepository == null)
                {
                    _bankAccountRepository = new GenericRepository<BankAccountEntity>(_context);
                }
                return _bankAccountRepository;
            }
        }

        public GenericRepository<NotificationEntity> NotificationRepository
        {
            get
            {
                if (_notificationRepository == null)
                {
                    _notificationRepository = new GenericRepository<NotificationEntity>(_context);
                }
                return _notificationRepository;
            }
        }

        public GenericRepository<TransactionHistoryEntity> TransactionHistoryRepository
        {
            get
            {
                if (_transactionHistoryRepository == null)
                {
                    _transactionHistoryRepository = new GenericRepository<TransactionHistoryEntity>(_context);
                }
                return _transactionHistoryRepository;
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
