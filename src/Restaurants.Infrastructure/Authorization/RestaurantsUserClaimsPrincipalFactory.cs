using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Entities;
using System.Security.Claims;

namespace Restaurants.Infrastructure.Authorization;

public class RestaurantsUserClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor) : UserClaimsPrincipalFactory<User>(userManager, optionsAccessor)
{
    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var principal = await base.CreateAsync(user);

        var identity = (ClaimsIdentity)principal.Identity!;

        if (user.Nationality != null)
        {
            identity.AddClaim(new Claim(AppClaimTypes.Nationality, user.Nationality));
        }
        if (user.DateOfBirth != null)
        {
            identity.AddClaim(new Claim(AppClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString("yyyy-MM-dd")));
        }

        var roles = await UserManager.GetRolesAsync(user);
        foreach (var role in roles)
            identity.AddClaim(new Claim(ClaimTypes.Role, role));


        return principal;
    }
}