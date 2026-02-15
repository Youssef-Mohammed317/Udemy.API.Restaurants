using Microsoft.AspNetCore.Identity;

namespace Restaurants.Domain.Exceptions;

public class NotFoundException(string resourceType, string resourceIdentifier)
    : Exception($"{resourceType} with id: {resourceIdentifier} doesn't exist.")

{
}
public sealed class IdentityOperationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public IdentityOperationException(string message, IdentityResult result)
        : base(message)
    {
        Errors = result.Errors
            .GroupBy(e => string.IsNullOrWhiteSpace(e.Code) ? "General" : e.Code)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Description).Distinct().ToArray()
            );
    }
}