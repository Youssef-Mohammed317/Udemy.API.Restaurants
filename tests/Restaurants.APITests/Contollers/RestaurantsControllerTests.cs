using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Contollers;
using Restaurants.APITests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance.Seeds.Abstractions;
using Restaurants.Infrastructure.Persistance.Seeds.Seeders;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace Restaurants.API.Contollers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly Mock<IUnitOfWork> fakeUnitOfWork = new();
    private Mock<IValidator<GetAllRestaurantsQuery>> validatorMock = new();
    private Mock<RestaurantSeeder> resturantSeederMock = new();
    private Mock<RoleSeeder> roleSeederMock = new();
    private Mock<CategorySeeder> categorySeederMock = new();
    private Mock<IDbInitializer> dbInitializerMock = new();


    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        this.factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IValidator<GetAllRestaurantsQuery>>();
                    services.AddSingleton<IValidator<GetAllRestaurantsQuery>>(_ => validatorMock.Object);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.Replace(
                    ServiceDescriptor.Scoped<IUnitOfWork>(_ => fakeUnitOfWork.Object));

                    services.Replace(ServiceDescriptor.Scoped<IEntitySeeder>(_ => roleSeederMock.Object));
                    services.Replace(ServiceDescriptor.Scoped<IEntitySeeder>(_ => categorySeederMock.Object));
                    services.Replace(ServiceDescriptor.Scoped<IEntitySeeder>(_ => resturantSeederMock.Object));
                    services.Replace(ServiceDescriptor.Scoped<IDbInitializer>(_ => dbInitializerMock.Object));

                });
        });
    }
    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        var pageSize = 10;
        var pageNumber = 1;
        // arrange

        validatorMock
             .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetAllRestaurantsQuery>>(),
                                         It.IsAny<CancellationToken>()))
             .ReturnsAsync((ValidationContext<GetAllRestaurantsQuery> ctx, CancellationToken _) =>
             {
                 var req = ctx.InstanceToValidate;
                 var failures = new List<ValidationFailure>();

                 if (req.PageSize <= 0)
                     failures.Add(new ValidationFailure(nameof(req.PageSize), "PageSize must be greater than 0"));

                 if (req.PageNumber <= 0)
                     failures.Add(new ValidationFailure(nameof(req.PageNumber), "PageNumber must be greater than 0"));

                 return new ValidationResult(failures);
             });

        var client = factory.CreateClient();
        fakeUnitOfWork.Setup(u => u.RestaurantRepository.GetAllAsync(null, null, null, pageSize, pageNumber, false)).ReturnsAsync([]);

        // act
        var result = await client.GetAsync($"/api/restaurants?PageNumber={pageNumber}&PageSize={pageSize}");
        // assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

    }
    [Fact()]
    public async Task GetAll_ForInValidRequest_Returns400BadRequest()
    {
        var pageSize = -1;
        var pageNumber = -1;
        // arrange
        var client = factory.CreateClient();

        validatorMock
             .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GetAllRestaurantsQuery>>(),
                                         It.IsAny<CancellationToken>()))
             .ReturnsAsync(new ValidationResult(new[]{
                 new ValidationFailure("PageSize", "PageSize must be greater than 0"),
                 new ValidationFailure("pageNumber", "PageSize must be greater than 0")
                     }));

        fakeUnitOfWork.Setup(u => u.RestaurantRepository.GetAllAsync(null, null, null, pageSize, pageNumber, false))
                .ReturnsAsync([]);

        // act
        var result = await client.GetAsync($"/api/restaurants");
        // assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

    }

    [Fact()]
    public async Task GetById_ForValidRequest_Returns200Ok()
    {
        var id = 5;
        // arrange
        var client = factory.CreateClient();
        var rest = new Restaurant
        {
            Id = id,
            Name = "Test Restaurant",
            Description = "A test restaurant",
        };
        fakeUnitOfWork.Setup(u => u.RestaurantRepository.GetByIdAsync(id)).ReturnsAsync(rest);
        // act
        var result = await client.GetAsync($"/api/restaurants/{id}");

        var restaurantDto = await result.Content.ReadFromJsonAsync<RestaurantDto>();


        // assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(restaurantDto);
        Assert.Equal(restaurantDto!.Id, id);
        Assert.Equal(restaurantDto.Name, rest.Name);
        Assert.Equal(restaurantDto.Description, rest.Description);
    }
    [Fact()]
    public async Task GetById_ForInValidRequest_Returns404NotFound()
    {
        var id = 999;
        // arrange
        var client = factory.CreateClient();
        fakeUnitOfWork.Setup(u => u.RestaurantRepository.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

        // act
        var result = await client.GetAsync($"/api/restaurants/{id}");
        // assert
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

}