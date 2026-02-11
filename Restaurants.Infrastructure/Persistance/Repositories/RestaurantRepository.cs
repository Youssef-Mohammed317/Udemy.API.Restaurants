using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Entities.Base;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance.Contexts;
namespace Restaurants.Infrastructure.Persistance.Repositories;

public class UnitOfWork(RestaurantsDbContext _context) : IUnitOfWork
{
    private IRestaurantRepository? _restaurantRepository;
    private IDishRepository? _dishRepository;
    private ICategoryRepository? _categoryRepository;

    public IRestaurantRepository RestaurantRepository => _restaurantRepository ??= new RestaurantRepository(_context);
    public IDishRepository DishRepository => _dishRepository ??= new DishRepository(_context);
    public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
    

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();

    }
}
public class Repository<TEntity, TKey>(RestaurantsDbContext _context) : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    public virtual async Task AddAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _context.Set<TEntity>().Remove(entity);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var entities = await _context.Set<TEntity>().ToListAsync();
        return entities!;
    }

    public virtual async Task<TEntity?> GetById(TKey id)
    {

        ArgumentNullException.ThrowIfNull(id);
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public virtual void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _context.Set<TEntity>().Update(entity);
    }
}

public class RestaurantRepository(RestaurantsDbContext _context) : Repository<Restaurant, int>(_context), IRestaurantRepository
{
}
public class CategoryRepository(RestaurantsDbContext _context) : Repository<Category, int>(_context), ICategoryRepository
{
}
public class DishRepository(RestaurantsDbContext _context) : Repository<Dish, int>(_context), IDishRepository
{
}
