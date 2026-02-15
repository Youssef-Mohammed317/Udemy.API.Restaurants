using FluentValidation;

namespace Restaurants.Application.Users.Commands.UpdateUserDetails;

public class UpdateUserDetailsCommandValidator : AbstractValidator<UpdateUserDetailsCommand>
{
    public UpdateUserDetailsCommandValidator()
    {
        RuleFor(x => x.DateOfBirth)
            .Must(BeInThePast)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("DateOfBirth must be in the past.")
            .Must(BeAtLeast13YearsOld)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("User must be at least 13 years old.");

        RuleFor(x => x.Nationality)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .When(x => x.Nationality is not null)
            .WithMessage("Nationality cannot be empty.")
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches(@"^[\p{L}\s'-]+$")
            .When(x => x.Nationality is not null)
            .WithMessage("Nationality contains invalid characters.");
    }

    private static bool BeInThePast(DateOnly? dob)
    {
        if (!dob.HasValue) return true;
        return dob.Value < DateOnly.FromDateTime(DateTime.UtcNow);
    }

    private static bool BeAtLeast13YearsOld(DateOnly? dob)
    {
        if (!dob.HasValue) return true;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var cutoff = today.AddYears(-13);

        return dob.Value <= cutoff;
    }
}