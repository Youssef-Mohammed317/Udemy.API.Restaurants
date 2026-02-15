using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance;
namespace Restaurants.Infrastructure.Persistance.Repositories;

public class RestaurantRepository(RestaurantsDbContext _context) : Repository<Restaurant, int>(_context), IRestaurantRepository
{
}
