using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Categories.Dtos;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public List<RestaurantDto?>? Restaurants { get; set; } = new();
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
