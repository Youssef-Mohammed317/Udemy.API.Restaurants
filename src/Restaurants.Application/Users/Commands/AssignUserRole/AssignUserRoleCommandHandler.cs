using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> _logger,
    UserManager<User> _userManager, RoleManager<IdentityRole> _roleManager
    ) : IRequestHandler<AssignUserRoleCommand>
{

    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Assigning role {RoleName} to user with email {UserEmail}", request.RoleName, request.UserEmail);

        var user = await _userManager.FindByEmailAsync(request.UserEmail)
            ??
            throw new NotFoundException(nameof(User), request.UserEmail);

        var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
        if (!roleExists)
        {
            throw new NotFoundException(nameof(IdentityRole), request.RoleName);
        }

        var result = await _userManager.AddToRoleAsync(user, request.RoleName);
        if (!result.Succeeded)
            throw new IdentityOperationException($"Failed to assign role {request.RoleName} to {request.UserEmail}", result);

    }
}
