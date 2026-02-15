using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumRestaurantsRequirementHandler(ILogger<MinimumRestaurantsRequirementHandler> _logger,
    IUserContext userContext,
    IUnitOfWork unitOfWork
    ) : AuthorizationHandler<MinimumRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        MinimumRestaurantsRequirement requirement)
    {
        var user = userContext.GetCurrentUser();
        if (user == null)
        {
            context.Fail();
            return;
        }

        _logger.LogInformation("User: {email} - Handling MinimumRestaurantsRequirement",
            user.Email);

        var restaurantsNumber = await unitOfWork.RestaurantRepository.CountAsync(r => r.OwnerId == user.Id);

        if (restaurantsNumber >= requirement.MinimumRestaurants)
        {
            _logger.LogInformation("Authorization requirement succeeded for user: {email}", user.Email);
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        return;

    }
}
