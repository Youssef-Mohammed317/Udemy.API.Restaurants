using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos.Validators;
using Restaurants.Application.Services;
using Restaurants.Application.Services.Implementations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Restaurants.Application;

public static class RegisterApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var applicationAssembly = typeof(RegisterApplicationServices).Assembly;

        services.AddScoped<IServiceManager, ServiceManager>();

        services.AddScoped<IRestaurantService, RestaurantService>();
        services.AddScoped<Func<IRestaurantService>>(provider =>
                    () => provider.GetRequiredService<IRestaurantService>());


        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(applicationAssembly);
        });
        services.AddValidatorsFromAssembly(applicationAssembly);
    }
}
