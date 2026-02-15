using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UnAssingUserRole;

public class UnAssignUserRoleCommandHandler(ILogger<UnAssignUserRoleCommandHandler> _logger,
    UserManager<User> _userManager, RoleManager<IdentityRole> _roleManager
    ) : IRequestHandler<UnAssignUserRoleCommand>
{

    public async Task Handle(UnAssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Un Assigning role {RoleName} from user with email {UserEmail}",
            request.RoleName, request.UserEmail);

        var user = await _userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

        if (!await _roleManager.RoleExistsAsync(request.RoleName))
            throw new NotFoundException(nameof(IdentityRole), request.RoleName);

        var isInRole = await _userManager.IsInRoleAsync(user, request.RoleName);
        if (!isInRole)
            throw new NotFoundException("UserRole", $"{request.UserEmail} is not in role {request.RoleName}");

        var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
        if (!result.Succeeded)
            throw new IdentityOperationException($"Failed to remove role {request.RoleName} from {request.UserEmail}", result);
    }
}

