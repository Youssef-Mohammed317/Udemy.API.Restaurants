using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteRestaurantDish;

public record DeleteRestaurantDishCommand(int RestaurantId, int Id) : IRequest;
