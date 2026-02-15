namespace Restaurants.Infrastructure.Persistance.Seeds.Abstractions;

public interface IEntitySeeder
{
    Task SeedAsync();
}
