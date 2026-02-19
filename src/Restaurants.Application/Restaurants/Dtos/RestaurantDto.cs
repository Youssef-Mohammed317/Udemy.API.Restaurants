using Restaurants.Application.Categories.Dtos;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool HasDelivery { get; set; }
    public int CategoryId { get; set; }

    public string? ContactEmail { get; set; }
    public string? ContactNumber { get; set; }

    public string? LogoSasUrl { get; set; }

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }

    public string OwnerId { get; set; } = default!;

    public CategoryDto? Category { get; set; } = default!;
    public List<DishDto?>? Dishes { get; set; } = new();

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
