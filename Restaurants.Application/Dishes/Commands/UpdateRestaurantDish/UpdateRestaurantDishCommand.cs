using MediatR;

namespace Restaurants.Application.Dishes.Commands.UpdateRestaurantDish;

public class UpdateRestaurantDishCommand : IRequest
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }
}
