using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteAllRestaurantDishes;

public record DeleteAllRestaurantDishesCommand(int RestaurantId) : IRequest;
