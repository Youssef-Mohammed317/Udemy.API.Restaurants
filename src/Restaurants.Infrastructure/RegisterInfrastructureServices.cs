using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Persistance;
using Restaurants.Infrastructure.Persistance.Repositories;
using Restaurants.Infrastructure.Persistance.Seeds;
using Restaurants.Infrastructure.Persistance.Seeds.Abstractions;
using Restaurants.Infrastructure.Persistance.Seeds.Seeders;
using Restaurants.Infrastructure.Storage;


namespace Restaurants.Infrastructure;

public static class RegisterInfrastructureServices
{
    public static void AddInfrastructureServices(IServiceCollection services, IConfiguration configurations)
    {

        services.AddDbContext<RestaurantsDbContext>(options =>
        {
            options.UseSqlServer(configurations.GetConnectionString("RestaurantsDbConnection"))
            .EnableSensitiveDataLogging();
        });


        services.AddScoped<IEntitySeeder, RoleSeeder>();
        services.AddScoped<IEntitySeeder, CategorySeeder>();
        services.AddScoped<IEntitySeeder, RestaurantSeeder>();

        services.AddScoped<IDbInitializer, DbInitializer>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();


        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();

        services.Configure<BlobStorageSettings>(configurations.GetSection("BlobStorageSettings"));
        services.AddScoped<IBlobStorageService, BlobStorageService>();





    }
}
