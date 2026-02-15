using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Persistance;
using Serilog;

namespace Restaurants.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
            });
            c.AddSecurityRequirement((document) => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("BearerAuth", document)] = []
            });
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();





        builder.Host.UseSerilog((context, config) =>
        {
            config.ReadFrom.Configuration(context.Configuration);
        });




        builder.Services
            .AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasNationality, policy =>
            {
                policy.RequireClaim(AppClaimTypes.Nationality, "German", "Polish"); // check if the user has a claim "Nationality" with value "German" or "Polish"
            })
            .AddPolicy(PolicyNames.AtLeast20, policy =>
            {
                policy.AddRequirements(new MinimumAgeRequirement(20)); // check if the user is at least 20 years old
            })
            .AddPolicy(PolicyNames.AtLeastOwnerOf2Restaurants, policy =>
            {
                policy.AddRequirements(new MinimumRestaurantsRequirement(2)); // check if the user is at least have 2 restaurants
            })
            ;
        builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, MinimumRestaurantsRequirementHandler>();
    }
}
