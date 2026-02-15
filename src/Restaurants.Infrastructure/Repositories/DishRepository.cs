using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
namespace Restaurants.Infrastructure.Persistance.Repositories;

public class DishRepository(RestaurantsDbContext _context) : Repository<Dish, int>(_context), IDishRepository
{
}
