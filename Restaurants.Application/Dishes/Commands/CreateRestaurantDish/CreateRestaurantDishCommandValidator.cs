using FluentValidation;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateRestaurantDish;

public class CreateRestaurantDishCommandValidator : AbstractValidator<CreateRestaurantDishCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRestaurantDishCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

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

        RuleFor(x => x.RestaurantId)
            .MustAsync(RestaurantExist)
            .WithMessage("Please insert a valid restaurant");
    }

    private async Task<bool> RestaurantExist(int restaurantId, CancellationToken token)
    {
        return await _unitOfWork.RestaurantRepository.CheckExistAsync(r => r.Id == restaurantId);
    }
}
