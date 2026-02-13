using AutoMapper;
using Restaurants.Application.Dishes.Commands.CreateRestaurantDish;
using Restaurants.Application.Dishes.Commands.UpdateRestaurantDish;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.Dtos;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<Dish, DishDto>();
        CreateMap<CreateRestaurantDishCommand, Dish>();
        CreateMap<UpdateRestaurantDishCommand, Dish>();
    }
}
