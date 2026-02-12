using Restaurants.Application.Categories.Dtos;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;

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

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }

    public CategoryDto? Category { get; set; } = default!;
    public List<DishDto?>? Dishes { get; set; } = new();

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public static RestaurantDto? FromEntity(Restaurant? restaurant)
    {
        if (restaurant == null) return null;
        return new RestaurantDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Description = restaurant.Description,
            HasDelivery = restaurant.HasDelivery,
            ContactEmail = restaurant.ContactEmail,
            ContactNumber = restaurant.ContactNumber,
            Street = restaurant.Address?.Street,
            City = restaurant.Address?.City,
            PostalCode = restaurant.Address?.PostalCode,
            CategoryId = restaurant.CategoryId,
            Category = CategoryDto.FromEntity(restaurant.Category),
            CreatedAt = restaurant.CreatedAt,
            UpdatedAt = restaurant.UpdatedAt,
            Dishes = restaurant.Dishes?.Select(DishDto.FromEntity).ToList()
        };
    }
}
