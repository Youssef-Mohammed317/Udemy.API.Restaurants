using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Services;

public interface IServiceManager
{
    IRestaurantService RestaurantService { get; }
}

