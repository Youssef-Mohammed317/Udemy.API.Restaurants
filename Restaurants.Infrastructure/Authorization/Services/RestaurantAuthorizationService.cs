using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(
    ILogger<RestaurantAuthorizationService> _logger,
    IUserContext _userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation operation)
    {
        var user = _userContext.GetCurrentUser()
            ?? throw new InvalidOperationException("User context is not available");

        _logger.LogInformation("Authorizing user {UserEmail} for operation {Operation} on restaurant {RestaurantName}",
            user.Email, operation, restaurant.Name);

        if (operation == ResourceOperation.Read || operation == ResourceOperation.Create)
        {
            _logger.LogInformation("Operation {Operation} is allowed for all users", operation);
            return true;
        }

        if (operation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            _logger.LogInformation("Operation {Operation} is allowed for admin users", operation);
            return true;
        }

        if ((operation == ResourceOperation.Update || operation == ResourceOperation.Delete)
            && restaurant.OwnerId == user.Id)
        {
            _logger.LogInformation("Operation {Operation} is allowed for restaurant owners", operation);
            return true;
        }

        return false;

    }
}