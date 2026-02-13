using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.UpdateRestaurantDish;

public class UpdateRestaurantDishCommandValidator : AbstractValidator<UpdateRestaurantDishCommand>
{

    public UpdateRestaurantDishCommandValidator()
    {

        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.RestaurantId)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Description)
                .NotEmpty()
                .Must(d => !string.IsNullOrWhiteSpace(d));

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.KiloCalories)
            .GreaterThan(0)
            .WithMessage("KiloCalories must be greater than 0 when provided.")
            .When(x => x.KiloCalories.HasValue);
    }

}

