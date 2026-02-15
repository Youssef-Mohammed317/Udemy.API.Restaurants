namespace Restaurants.Infrastructure.Persistance.Seeds.Abstractions;

public interface IDbInitializer
{
    Task InitializeAsync();
}
