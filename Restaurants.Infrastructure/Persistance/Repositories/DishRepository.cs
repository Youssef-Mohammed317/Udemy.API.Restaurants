using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance.Contexts;
namespace Restaurants.Infrastructure.Persistance.Repositories;

public class DishRepository(RestaurantsDbContext _context) : Repository<Dish, int>(_context), IDishRepository
{
}
