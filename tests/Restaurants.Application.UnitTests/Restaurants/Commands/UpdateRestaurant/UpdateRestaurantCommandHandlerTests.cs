using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests
{
    public class UpdateRestaurantCommandHandlerTests
    {
        private Mock<IRestaurantAuthorizationService> authServiceMock;
        private UpdateRestaurantCommandHandler handler;
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IRestaurantRepository> restaurantRepositoryMock;
        private Mock<ILogger<UpdateRestaurantCommandHandler>> loggerMock;
        private Mock<IMapper> mapperMock;
        private UpdateRestaurantCommand command;
        private Restaurant restaurant;

        public UpdateRestaurantCommandHandlerTests()
        {
            loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
            mapperMock = new Mock<AutoMapper.IMapper>();

            command = new UpdateRestaurantCommand()
            {
                Description = "Test Description",
                HasDelivery = true,
                Id = 1,
                Name = "Test Name"
            };
            restaurant = new Restaurant()
            {
                Description = "Test old Description",
                HasDelivery = false,
                Id = 1,
                Name = "Test old Name"
            };
            mapperMock.Setup(m => m.Map(command, restaurant)).Callback(() =>
            {
                restaurant.Name = command.Name;
                restaurant.Description = command.Description;
                restaurant.HasDelivery = command.HasDelivery;
            });

            unitOfWorkMock = new Mock<IUnitOfWork>();
            restaurantRepositoryMock = new Mock<IRestaurantRepository>();
            restaurantRepositoryMock.Setup(r => r.Update(restaurant));
            authServiceMock = new Mock<IRestaurantAuthorizationService>();

            handler = new UpdateRestaurantCommandHandler(loggerMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object,
                authServiceMock.Object);

        }
        [Fact()]
        public void Handle_UpdateRestaurantCommandHandlerForValid_Test()
        {

            restaurantRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(restaurant);
            unitOfWorkMock.Setup(u => u.RestaurantRepository).Returns(restaurantRepositoryMock.Object);
            unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            authServiceMock.Setup(a => a.Authorize(restaurant, ResourceOperation.Update)).Returns(true);

            var handler = new UpdateRestaurantCommandHandler(loggerMock.Object,
                unitOfWorkMock.Object,
                mapperMock.Object,
                authServiceMock.Object);

            // act
            var result = handler.Handle(command, CancellationToken.None);
            // assert
            Assert.Equal(command.Description, restaurant.Description);
            Assert.Equal(command.HasDelivery, restaurant.HasDelivery);
            Assert.Equal(command.Name, restaurant.Name);
            Assert.Equal(command.Id, restaurant.Id);
            Assert.Equal("Test Description", restaurant.Description);
            Assert.True(restaurant.HasDelivery);
            Assert.Equal("Test Name", restaurant.Name);

            authServiceMock.Verify(a => a.Authorize(restaurant, ResourceOperation.Update), Times.Once);
            mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
            restaurantRepositoryMock.Verify(r => r.Update(restaurant), Times.Once);
            unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }
        [Fact()]
        public void Handle_UpdateRestaurantCommandHandlerForNotFoundExceptionValid_Test()
        {

            restaurantRepositoryMock.Setup(r => r.GetByIdAsync(2));
            unitOfWorkMock.Setup(u => u.RestaurantRepository).Returns(restaurantRepositoryMock.Object);

            // act
            var result = handler.Handle(command, CancellationToken.None);
            // assert

            Assert.Throws<NotFoundException>(() => result.GetAwaiter().GetResult()).Message.Contains("Restaurant with Id => 1 was not found.");



        }
        [Fact()]
        public void Handle_UpdateRestaurantCommandHandlerForForbiddenExceptionValid_Test()
        {

            restaurantRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(restaurant);
            unitOfWorkMock.Setup(u => u.RestaurantRepository).Returns(restaurantRepositoryMock.Object);
            authServiceMock.Setup(a => a.Authorize(restaurant, ResourceOperation.Update)).Returns(false);

            // act
            var result = handler.Handle(command, CancellationToken.None);
            // assert

            Assert.Throws<ForbiddenException>(() => result.GetAwaiter().GetResult()).Message.Contains("Forbidden exception");

        }


    }
}