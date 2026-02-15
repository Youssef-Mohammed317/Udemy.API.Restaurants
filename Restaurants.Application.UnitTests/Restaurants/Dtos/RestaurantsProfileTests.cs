using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Restaurants.Application.Restaurants.Dtos.Tests;

public class RestaurantsProfileTests
{
    private IMapper mapper;

    public RestaurantsProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        }, NullLoggerFactory.Instance);

        mapper = config.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForRestaurantDto_MapsCorrectly()
    {
        // arrange
        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "A test restaurant",
            ContactEmail = "test@example.com",
            CategoryId = 2,
            Address = new Address
            {
                Street = "123 Test St",
                City = "Testville",
                PostalCode = "123-45"
            },
            ContactNumber = "01228616799",
            HasDelivery = true,
        };

        // act 
        var restaurantDto = mapper.Map<RestaurantDto>(restaurant);

        // assert
        Assert.NotNull(restaurantDto);
        Assert.Equal(restaurant.Id, restaurantDto.Id);
        Assert.Equal(restaurant.Name, restaurantDto.Name);
        Assert.Equal(restaurant.Description, restaurantDto.Description);
        Assert.Equal(restaurant.ContactEmail, restaurantDto.ContactEmail);
        Assert.Equal(restaurant.CategoryId, restaurantDto.CategoryId);
        Assert.Equal(restaurant.Address.Street, restaurantDto.Street);
        Assert.Equal(restaurant.Address.City, restaurantDto.City);
        Assert.Equal(restaurant.Address.PostalCode, restaurantDto.PostalCode);
        Assert.Equal(restaurant.HasDelivery, restaurantDto.HasDelivery);

    }
    [Fact()]
    public void CreateMap_CreateRestaurantCommandToRestaurant_MapsCorrectly()
    {

        // arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "A test restaurant",
            ContactEmail = "test@example.com",
            CategoryId = 2,
            Street = "123 Test St",
            City = "Testville",
            PostalCode = "123-45",
            ContactNumber = "01228616799",
            HasDelivery = true,
        };

        // act 
        var restaurant = mapper.Map<Restaurant>(command);

        // assert
        Assert.NotNull(restaurant);
        Assert.Equal(command.Name, restaurant.Name);
        Assert.Equal(command.Description, restaurant.Description);
        Assert.Equal(command.ContactEmail, restaurant.ContactEmail);
        Assert.Equal(command.CategoryId, restaurant.CategoryId);
        Assert.Equal(command.Street, restaurant.Address.Street);
        Assert.Equal(command.City, restaurant.Address.City);
        Assert.Equal(command.PostalCode, restaurant.Address.PostalCode);
        Assert.Equal(command.HasDelivery, restaurant.HasDelivery);

    }
    [Fact()]
    public void CreateMap_UpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange
        var command = new UpdateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "A test restaurant",
            HasDelivery = true,
            Id = 1
        };

        // act 
        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "Old Name",
            Description = "Old Description",
            HasDelivery = false,
        };
        mapper.Map(command, restaurant);

        // assert
        Assert.NotNull(restaurant);
        Assert.Equal(command.Id, restaurant.Id);
        Assert.Equal(command.Name, restaurant.Name);
        Assert.Equal(command.Description, restaurant.Description);
        Assert.Equal(command.HasDelivery, restaurant.HasDelivery);

    }

}