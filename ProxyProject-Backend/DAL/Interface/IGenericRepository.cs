using System.Linq.Expressions;

namespace ProxyProject_Backend.DAL.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int page = 0, int take = 0);
        Task<TEntity> GetByFilterAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetByIDAsync(object id);
        Task<TEntity> InsertAsync(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        Task<int> CountByFilterAsync(Expression<Func<TEntity, bool>> filter = null);
        Task InsertListAsync(List<TEntity> entities);
        void DeleteList(List<Guid> ids);
        void DeleteList(List<TEntity> entitiesToDelete);
        void UpdateList(List<TEntity> entitiesToUpdate);
    }
}
