using FluentValidation;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] _allowedPageSizes = [5, 10, 15, 30];
    private string[] _allowedSortByColumnNames = [nameof(Restaurant.Name),
        nameof(Restaurant.Description),
        nameof(Restaurant.CategoryId)];
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber).GreaterThan(0);
        RuleFor(r => r.PageSize)
            .Must(value => _allowedPageSizes.Contains(value))
            .WithMessage($"PageSize must be in [{string.Join(", ", _allowedPageSizes)}]");
        RuleFor(r => r.SortBy)
            .Must(value => _allowedSortByColumnNames.Contains(value))
            .When(q => q.SortBy != null)
            .WithMessage($"SortBy is Optional, or must be in [{string.Join(", ", _allowedSortByColumnNames)}]");
    }
}
