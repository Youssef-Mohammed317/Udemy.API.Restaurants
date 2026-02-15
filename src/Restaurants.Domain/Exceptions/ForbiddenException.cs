namespace Restaurants.Domain.Exceptions;

public class ForbiddenException(string resourceType, string resourceIdentifier, string action) 
    : Exception($"Forbidden: you are not allowed to {action} {resourceType} (Id: {resourceIdentifier}).")
{

}