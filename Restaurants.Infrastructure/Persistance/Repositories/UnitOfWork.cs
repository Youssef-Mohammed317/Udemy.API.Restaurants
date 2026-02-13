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
