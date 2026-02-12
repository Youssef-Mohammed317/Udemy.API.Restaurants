using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Services.Implementations;

public class ServiceManager(Func<IRestaurantService> _restaurantServiceFactory) : IServiceManager
{
    public IRestaurantService RestaurantService => _restaurantServiceFactory.Invoke();
}
