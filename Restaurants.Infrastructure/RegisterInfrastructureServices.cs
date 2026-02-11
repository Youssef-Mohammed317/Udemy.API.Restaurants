using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance.Contexts;
using Restaurants.Infrastructure.Persistance.Repositories;
using Restaurants.Infrastructure.Persistance.Seeds;
using Restaurants.Infrastructure.Persistance.Seeds.Abstractions;
using Restaurants.Infrastructure.Persistance.Seeds.Seeders;

namespace Restaurants.Infrastructure;

public static class RegisterInfrastructureServices
{
    public static void AddInfrastructureServices(IServiceCollection services, IConfiguration configurations)
    {

        services.AddDbContext<RestaurantsDbContext>(options =>
        {
            options.UseSqlServer(configurations.GetConnectionString("RestaurntsDbConnection"));
        });

        services.AddScoped<IEntitySeeder, CategorySeeder>();
        services.AddScoped<IEntitySeeder, RestaurantSeeder>();
        services.AddScoped<IEntitySeeder, DishSeeder>();
        services.AddScoped<IDbInitializer, DbInitializer>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
