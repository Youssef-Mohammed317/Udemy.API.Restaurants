using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Entities.Base;

namespace Restaurants.Domain.Entities;

public class User : IdentityUser, IAuditableEntity
{

    public DateOnly? DateOfBirth { get; set; }

    public string? Nationality { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<Restaurant> OwnedRestaurants { get; set; } = new();
}
