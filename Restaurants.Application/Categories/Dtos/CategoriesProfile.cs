using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Categories.Dtos;

public class CategoriesProfile : Profile
{
    public CategoriesProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}
