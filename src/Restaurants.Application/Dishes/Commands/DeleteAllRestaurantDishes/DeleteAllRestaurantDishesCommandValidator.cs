using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.DeleteAllRestaurantDishes;

public class DeleteAllRestaurantDishesCommandValidator : AbstractValidator<DeleteAllRestaurantDishesCommand>
{
    public DeleteAllRestaurantDishesCommandValidator()
    {
        RuleFor(x => x.RestaurantId)
            .GreaterThan(0);
    }
}
