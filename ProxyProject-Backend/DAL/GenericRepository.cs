using Microsoft.EntityFrameworkCore;
using ProxyProject_Backend.DAL.Interface;
using System.Linq.Expressions;

namespace ProxyProject_Backend.DAL
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ApplicationDbContext _context;
        internal DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int page = 0, int take = 0)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                if(take != 0)
                {
                    return await orderBy(query).Skip(page * take).Take(take).ToListAsync();
                }
                else
                {
                    return await orderBy(query).Skip(page).ToListAsync();
                }
            }
            else
            {
                if (take != 0)
                {
                    return await query.Skip(page).Take(take).ToListAsync();
                }
                else
                {
                    return await query.Skip(page).ToListAsync();
                }
            }
        }

        public virtual async Task<TEntity> GetByFilterAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var trackingEntity = await _dbSet.AddAsync(entity);
            return trackingEntity.Entity;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task<int> CountByFilterAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter != null ? await _dbSet.CountAsync(filter) : await _dbSet.CountAsync();
        }

        public async Task InsertListAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void DeleteList(List<Guid> ids)
        {
            var entitiesToDelete = new List<TEntity>();

            ids.ForEach(id =>
            {
                entitiesToDelete.Add(_dbSet.Find(id));
            });

            DeleteList(entitiesToDelete);
        }

        public void DeleteList(List<TEntity> entitiesToDelete)
        {
            entitiesToDelete.ForEach(entityToDelete =>
            {
                if (_context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }
            });

            _dbSet.RemoveRange(entitiesToDelete);
        }

        public void UpdateList(List<TEntity> entitiesToUpdate)
        {
            entitiesToUpdate.ForEach(entityToUpdate =>
            {
                _dbSet.Attach(entityToUpdate);
                _context.Entry(entityToUpdate).State = EntityState.Modified;
            });
        }
    }
}
