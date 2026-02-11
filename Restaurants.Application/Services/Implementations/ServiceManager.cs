using Restaurants.Application.Services.Abstractions;

namespace Restaurants.Application.Services.Implementations;

public class ServiceManager(Func<IRestaurantService> _restaurantServiceFactory) : IServiceMaanager
{
    public IRestaurantService RestaurantService => _restaurantServiceFactory.Invoke();
}
public class RestaurantService : IRestaurantService
{

}
