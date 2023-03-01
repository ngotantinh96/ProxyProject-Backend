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

        private GenericRepository<ProxyEntity> _proxyRepository;

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

        public async Task Save()
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
