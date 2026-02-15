using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Persistance.Seeds.Abstractions;

namespace Restaurants.Infrastructure.Persistance.Seeds.Seeders;

public class RoleSeeder(RestaurantsDbContext context) : IEntitySeeder
{

    public async Task SeedAsync()
    {
        if (await context.Roles.AnyAsync())
            return;

        await context.Roles.AddRangeAsync(GetRoles());

        await context.SaveChangesAsync();
    }

    private IdentityRole[] GetRoles()
    {
        return new[]
        {
            new IdentityRole { Name = UserRoles.Admin, NormalizedName = UserRoles.Admin.ToUpper() },
            new IdentityRole { Name = UserRoles.User, NormalizedName = UserRoles.User.ToUpper() },
            new IdentityRole { Name = UserRoles.Owner, NormalizedName = UserRoles.Owner.ToUpper() },
        };
    }
}
