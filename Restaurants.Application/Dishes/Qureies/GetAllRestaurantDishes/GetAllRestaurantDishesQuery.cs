using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Qureies.GetAllRestaurantDishes;

public record GetAllRestaurantDishesQuery(int RestaurantId) : IRequest<IEnumerable<DishDto>>;
