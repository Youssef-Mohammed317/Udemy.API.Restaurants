using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.DeleteRestaurantDish;

public class DeleteRestaurntDishCommandValidator : AbstractValidator<DeleteRestaurantDishCommand>
{
    public DeleteRestaurntDishCommandValidator()
    {

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid dish id.");
        RuleFor(x => x.RestaurantId)
            .GreaterThan(0).WithMessage("Invalid restaurant id.");
    }
}