using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.Dtos;

public class DishDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }

    public int? KiloCalories { get; set; }
    public int RestaurantId { get; set; }
    public RestaurantDto? Restaurant { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static DishDto? FromEntity(Dish dish)
    {
        if (dish == null) return null;
        return new DishDto
        {
            Id = dish.Id,
            Name = dish.Name,
            Description = dish.Description,
            KiloCalories = dish.KiloCalories,
            Price = dish.Price,
            RestaurantId = dish.RestaurantId,
            CreatedAt = dish.CreatedAt,
            UpdatedAt = dish.UpdatedAt,
            Restaurant = RestaurantDto.FromEntity(dish.Restaurant)
        };
    }
}