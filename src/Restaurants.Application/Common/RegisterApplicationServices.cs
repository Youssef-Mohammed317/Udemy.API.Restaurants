using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Users;

namespace Restaurants.Application.Common;

public static class RegisterApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var applicationAssembly = typeof(RegisterApplicationServices).Assembly;


        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(applicationAssembly);
        });
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<IUserContext, UserContext>();

        services.AddHttpContextAccessor();

    }
}
