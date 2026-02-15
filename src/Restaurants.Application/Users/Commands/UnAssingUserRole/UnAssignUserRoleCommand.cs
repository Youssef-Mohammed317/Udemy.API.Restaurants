using MediatR;

namespace Restaurants.Application.Users.Commands.UnAssingUserRole;

public class UnAssignUserRoleCommand : IRequest
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}
