using Restaurants.API.Extensions;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistance.Seeds.Abstractions;
using Restaurants.IoC;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();

builder.Services.RegisterIoCServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await dbInitializer.InitializeAsync();
}



app.UseMiddleware<RequestTimeLoggingMiddleware>();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();

app.MapControllers();

await app.RunAsync();


public partial class Program { } // for testing purposes