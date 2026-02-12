using AutoMapper;
using Restaurants.Domain.Entities;
using Restaurants.Domain.ValueObjects;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(dest => dest.City, opt =>
                    opt.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(dest => dest.Street, opt =>
                    opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(dest => dest.PostalCode,
                    opt => opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode));



        CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                    {
                        City = src.City,
                        Street = src.Street,
                        PostalCode = src.PostalCode
                    }));
    }
}
