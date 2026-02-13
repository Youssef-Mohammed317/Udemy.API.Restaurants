using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance.Contexts;
namespace Restaurants.Infrastructure.Persistance.Repositories;

public class CategoryRepository(RestaurantsDbContext _context) : Repository<Category, int>(_context), ICategoryRepository
{
}
