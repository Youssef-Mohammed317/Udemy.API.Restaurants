using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Services.Abstractions;
using Restaurants.Application.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurants.Application;

public static class RegisterApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantService, RestaurantService>();
        services.AddScoped<Func<IRestaurantService>>(provider =>
                    () => provider.GetRequiredService<IRestaurantService>());
    }
}
