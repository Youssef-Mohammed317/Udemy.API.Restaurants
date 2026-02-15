using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{

    public UpdateRestaurantCommandValidator()
    {
        RuleFor(x => x.Id)
             .GreaterThan(0).WithMessage("Invalid restaurant id.");

        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required.");

    }
}
