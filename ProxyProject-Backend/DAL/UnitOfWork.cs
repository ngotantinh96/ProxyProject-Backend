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

        private GenericRepository<ProxyPlanEntity> _proxyPlanRepository;

        public GenericRepository<ProxyPlanEntity> ProxyPlanRepository
        {
            get
            {
                if (_proxyPlanRepository == null)
                {
                    _proxyPlanRepository = new GenericRepository<ProxyPlanEntity>(_context);
                }
                return _proxyPlanRepository;
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
