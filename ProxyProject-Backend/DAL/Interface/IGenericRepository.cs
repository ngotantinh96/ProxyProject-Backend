using System.Linq.Expressions;

namespace ProxyProject_Backend.DAL.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<TEntity> GetByFilterAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetByIDAsync(object id);
        Task InsertAsync(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
    }
}
