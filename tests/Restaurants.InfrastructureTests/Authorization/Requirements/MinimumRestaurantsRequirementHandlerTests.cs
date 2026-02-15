using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements;
using System.Linq.Expressions;
using Xunit;
using Assert = Xunit.Assert;
namespace Restaurants.InfrastructureTests.Authorization.Requirements;

public class MinimumRestaurantsRequirementHandlerTests
{
    private readonly Mock<ILogger<MinimumRestaurantsRequirementHandler>> _logger;
    private readonly Mock<IUserContext> _userContext;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IRestaurantRepository> _restaurantRepo;
    private readonly CurrentUser _user;

    public MinimumRestaurantsRequirementHandlerTests()
    {
        _logger = new Mock<ILogger<MinimumRestaurantsRequirementHandler>>();
        _userContext = new Mock<IUserContext>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _restaurantRepo = new Mock<IRestaurantRepository>();

        _user = new CurrentUser("user-123", "test@test.com", [UserRoles.User], "German", new DateOnly(1990, 1, 1));

        _userContext.Setup(c => c.GetCurrentUser()).Returns(_user);
        _unitOfWork.SetupGet(u => u.RestaurantRepository).Returns(_restaurantRepo.Object);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserHasMinimumNumberOfRestaurant_ValidTest()
    {
        // arrange
        _restaurantRepo
            .Setup(r => r.CountAsync(It.IsAny<Expression<Func<Restaurant, bool>>>()))
            .ReturnsAsync(5);

        var requirement = new MinimumRestaurantsRequirement(2);
        var handler = new MinimumRestaurantsRequirementHandler(_logger.Object, _userContext.Object, _unitOfWork.Object);
        var context = new AuthorizationHandlerContext(new[] { requirement }, null, null);

        // act
        await handler.HandleAsync(context);

        // assert
        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserHasMinimumNumberOfRestaurant_NotValidTest()
    {
        // arrange
        _restaurantRepo
            .Setup(r => r.CountAsync(It.IsAny<Expression<Func<Restaurant, bool>>>()))
            .ReturnsAsync(1);

        var requirement = new MinimumRestaurantsRequirement(2);
        var handler = new MinimumRestaurantsRequirementHandler(_logger.Object, _userContext.Object, _unitOfWork.Object);
        var context = new AuthorizationHandlerContext(new[] { requirement }, null, null);

        // act
        await handler.HandleAsync(context);

        // assert
        Assert.False(context.HasSucceeded);
    }
}
