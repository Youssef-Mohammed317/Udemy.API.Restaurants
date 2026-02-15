using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistance;
using Restaurants.Infrastructure.Persistance.Seeds.Abstractions;

namespace Restaurants.Infrastructure.Persistance.Seeds.Seeders;

public class CategorySeeder(RestaurantsDbContext context) : IEntitySeeder
{

    public async Task SeedAsync()
    {
        if (await context.Categories.AnyAsync())
            return;

        await context.Categories.AddRangeAsync(GetCategories());

        await context.SaveChangesAsync();
    }
    private IEnumerable<Category> GetCategories()
    {
        List<Category> categories = new List<Category>
            {
                new Category { Name = "Fast Food" },
                new Category { Name = "Italian" },
                new Category { Name = "Japanese" },
                new Category { Name = "Chinese" },
                new Category { Name = "Indian" }
            };


        return categories;
    }

}
