using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandHandlerTests
    {

        [Fact()]
        public async Task Handle_ForValidCommand_ReturnsRestaurantId()
        {
            // arrange
            var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();

            var mapperMock = new Mock<IMapper>();

            var command = new CreateRestaurantCommand()
            {
                Description = "Test Description",
                HasDelivery = true,
                Name = "Test Name"
            };
            var restaurant = new Restaurant();

            mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(() =>
            {
                restaurant.Name = command.Name;
                restaurant.Description = command.Description;
                restaurant.HasDelivery = command.HasDelivery;
                return restaurant;
            });

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var restaurantRepositoryMock = new Mock<IRestaurantRepository>();

            restaurantRepositoryMock
                 .Setup(r => r.CreateAsync(It.IsAny<Restaurant>()))
            .Callback<Restaurant>((r) => r.Id = 1);

            unitOfWorkMock.SetupGet(u => u.RestaurantRepository).Returns(restaurantRepositoryMock.Object);
            unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            var userContextMock = new Mock<IUserContext>();

            var user = new CurrentUser("owner-123", "test@test.com", [UserRoles.Owner], null, null);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(user);


            var commandHandler = new CreateRestaurantCommandHandler(loggerMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object,
                userContextMock.Object);

            // act
            var result = await commandHandler.Handle(command, CancellationToken.None);
            // assert
            Assert.IsType<int>(result);
            Assert.Equal(restaurant.Id, result);
            Assert.Equal(user.Id, restaurant.OwnerId);
            Assert.Equal(command.Name, restaurant.Name);
            Assert.Equal(command.Description, restaurant.Description);
            Assert.Equal(command.HasDelivery, restaurant.HasDelivery);

            restaurantRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Restaurant>()), Times.Once);
            unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);

        }
    }
}