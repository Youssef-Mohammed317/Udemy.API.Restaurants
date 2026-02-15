using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace Restaurants.Application.Users;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor?.HttpContext?.User
            ?? throw new InvalidOperationException("User context is not present");

        if (user.Identity?.IsAuthenticated != true)
            return null;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return null;

        var email = user.FindFirstValue(ClaimTypes.Email);

        var roles = user.FindAll(ClaimTypes.Role)
                        .Select(c => c.Value)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();


        var nationality = user.FindFirstValue("Nationality");
        var dateOfBirthString = user.FindFirstValue("DateOfBirth");
        var dateOfBirth = string.IsNullOrWhiteSpace(dateOfBirthString) ? (DateOnly?)null : DateOnly.ParseExact(dateOfBirthString, "yyyy-MM-dd");

        return new CurrentUser(userId, email, roles, nationality, dateOfBirth);
    }
}
