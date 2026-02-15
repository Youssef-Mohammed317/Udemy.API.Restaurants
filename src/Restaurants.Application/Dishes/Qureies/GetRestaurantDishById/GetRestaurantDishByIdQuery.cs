using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Qureies.GetRestaurantDishById;

public record GetRestaurantDishByIdQuery(int RestaurantId, int Id) : IRequest<DishDto>;
