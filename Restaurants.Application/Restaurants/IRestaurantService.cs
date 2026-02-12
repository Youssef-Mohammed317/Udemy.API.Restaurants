using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantService
{
    Task<int> Create(CreateRestaurantDto dto);
    Task<IEnumerable<RestaurantDto?>> GetAllAsync();
    Task<RestaurantDto?> GetByIdAsync(int id);
}

