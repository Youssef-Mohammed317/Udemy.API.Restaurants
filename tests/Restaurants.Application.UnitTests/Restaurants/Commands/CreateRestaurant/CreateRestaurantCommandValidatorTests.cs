using FluentValidation.TestHelper;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

// i dont how to work wiht unit of work in unit test so done later
public class CreateRestaurantCommandValidatorTests
{
    private Mock<IUnitOfWork> uowMock;

    public CreateRestaurantCommandValidatorTests()
    {
        var restaurantRepo = new Mock<IRestaurantRepository>();
        restaurantRepo
            .Setup(r => r.CheckExistAsync(It.IsAny<Expression<Func<Restaurant, bool>>>()))
            .ReturnsAsync(false);

        var categoryRepo = new Mock<ICategoryRepository>();
        categoryRepo
            .Setup(r => r.CheckExistAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(true);

        uowMock = new Mock<IUnitOfWork>();
        uowMock.SetupGet(x => x.RestaurantRepository).Returns(restaurantRepo.Object);
        uowMock.SetupGet(x => x.CategoryRepository).Returns(categoryRepo.Object);

    }
    [Fact()]
    public async Task CreateRestaurantCommandValidator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "A test restaurant description.",
            ContactEmail = "testtest@gmail.com",
            ContactNumber = "01012345678",
            Street = "Test Street",
            City = "Test City",
            PostalCode = "12-345",
            HasDelivery = true,
            CategoryId = 1
        };

        var validator = new CreateRestaurantCommandValidator(uowMock.Object);

        //act
        var result = await validator.TestValidateAsync(command);

        //assert
        result.ShouldNotHaveAnyValidationErrors();


    }
    [Fact()]
    public async Task CreateRestaurantCommandValidator_ForNotValidCommand_ShouldHaveValidationErrors()
    {
        // arrange
        var command = new CreateRestaurantCommand
        {
            Name = "nt",
            Description = "A test restaurant description.",
            ContactEmail = "admin@test.com",
            ContactNumber = "01641234567",
            Street = "Test Street",
            City = "Test City",
            PostalCode = "12345",
            HasDelivery = true,
            CategoryId = 23
        };

        var validator = new CreateRestaurantCommandValidator(uowMock.Object);

        //act
        var result = await validator.TestValidateAsync(command);

        //assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        result.ShouldHaveValidationErrorFor(c => c.ContactNumber);
    }
}