using MediatR;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery : IRequest<PageResult<RestaurantDto>>
{
    public string? SearchPhrase { get; set; }
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; }

}
public enum SortDirection
{
    Ascending,
    Descending
}
