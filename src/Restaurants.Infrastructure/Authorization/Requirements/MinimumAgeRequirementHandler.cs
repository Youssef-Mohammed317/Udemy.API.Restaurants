using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> _logger,
    IUserContext userContext
    ) : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        var user = userContext.GetCurrentUser();
        if (user == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        _logger.LogInformation("User: {email}, date of birth: {dateOfBirth} - Handling MinimumAgeRequirement",
            user.Email, user.DateOfBirth);

        if (user.DateOfBirth == null)
        {
            _logger.LogWarning("Authorization requirement failed for user: {email} - Date of birth is not provided", user.Email);
            context.Fail();
            return Task.CompletedTask;
        }

        if (user.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.UtcNow))
        {
            _logger.LogInformation("Authorization requirement succeeded for user: {email}", user.Email);
            context.Succeed(requirement);
        }
        else
        {

            context.Fail();
        }
        return Task.CompletedTask;
    }
}
