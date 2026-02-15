using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Restaurants.Domain.Repositories;

public interface IRepository<TEntity, TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id);
    void Update(TEntity entity);
    Task CreateAsync(TEntity entity);
    void Delete(TEntity entity);
    Task<int> DeleteAllAsync(Expression<Func<TEntity, bool>> filter);
    Task<bool> CheckExistAsync(Expression<Func<TEntity, bool>> filter);

    Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = false);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> filter);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int pageSize = 10, int pageNumber = 1, bool disableTracking = true);
}
