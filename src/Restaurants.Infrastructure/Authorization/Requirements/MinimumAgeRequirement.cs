using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public record MinimumAgeRequirement(int MinimumAge) : IAuthorizationRequirement;
