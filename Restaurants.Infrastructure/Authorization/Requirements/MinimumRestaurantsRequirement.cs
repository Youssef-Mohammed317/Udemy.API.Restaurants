using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public record MinimumRestaurantsRequirement(int MinimumRestaurants) : IAuthorizationRequirement;
