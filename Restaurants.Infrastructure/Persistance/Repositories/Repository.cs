using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Restaurants.Domain.Entities.Base;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance.Contexts;
using System.Linq.Expressions;
namespace Restaurants.Infrastructure.Persistance.Repositories;

public class Repository<TEntity, TKey>(RestaurantsDbContext _context) : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    public virtual async Task CreateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _context.Set<TEntity>().Remove(entity);
    }
    public virtual async Task<int> DeleteAllAsync(Expression<Func<TEntity, bool>> filter)
    {
        ArgumentNullException.ThrowIfNull(filter);
        return await _context.Set<TEntity>().Where(filter).ExecuteDeleteAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
        )
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (disableTracking)
            query = query.AsNoTracking();

        if (filter != null)
            query = query.Where(filter);

        if (include != null)
            query = include.Invoke(query);

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await _context.Set<TEntity>().FindAsync(id);
    }
    public virtual async Task<TEntity?> GetFirstAsync(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true)
    {
        var query = _context.Set<TEntity>().Where(filter);

        if (disableTracking) query = query.AsNoTracking();

        if (include != null)
            query = include.Invoke(query);

        return await query.FirstOrDefaultAsync();
    }

    public virtual void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _context.Set<TEntity>().Update(entity);
    }

    public virtual async Task<bool> CheckExistAsync(
        Expression<Func<TEntity, bool>> filter)
    {
        return await _context.Set<TEntity>().AnyAsync(filter);
    }
}
