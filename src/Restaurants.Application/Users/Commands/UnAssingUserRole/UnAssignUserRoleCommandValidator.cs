using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Commands.UnAssingUserRole;

public class UnAssignUserRoleCommandValidator : AbstractValidator<UnAssignUserRoleCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UnAssignUserRoleCommandValidator(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        RuleFor(x => x.UserEmail)
            .NotEmpty().EmailAddress()
            .MustAsync(CheckEmailExist)
            .WithMessage("Email {PropertyValue} does not exist.");
        RuleFor(x => x.RoleName)
            .NotEmpty()
            .MustAsync(CheckRoleExist)
            .WithMessage("Role {PropertyValue} does not exist.")
            ;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    private async Task<bool> CheckRoleExist(string roleName, CancellationToken token)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    private async Task<bool> CheckEmailExist(string email, CancellationToken token)
    {
        return await _userManager.FindByEmailAsync(email) is not null;
    }
}
