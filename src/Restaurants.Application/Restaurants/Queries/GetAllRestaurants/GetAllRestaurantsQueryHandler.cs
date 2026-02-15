using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Linq.Expressions;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
public class GetAllRestaurantsQueryHandler(IUnitOfWork _unitOfWork,
    ILogger<GetAllRestaurantsQueryHandler> _logger,
    IMapper _mapper) : IRequestHandler<GetAllRestaurantsQuery, PageResult<RestaurantDto>>
{
    public async Task<PageResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all restaurants");

        Expression<Func<Restaurant, bool>> filter = r =>
            request.SearchPhrase == null
            || r.Name.ToLower().Contains(request.SearchPhrase.ToLower())
            || r.Description.ToLower().Contains(request.SearchPhrase.ToLower());

        Func<IQueryable<Restaurant>, IOrderedQueryable<Restaurant>>? orderBy = request.SortBy switch
        {
            nameof(Restaurant.Name) => request.SortDirection switch
            {
                SortDirection.Ascending => q => q.OrderBy(r => r.Name),
                SortDirection.Descending => q => q.OrderByDescending(r => r.Name),
                _ => null
            },

            nameof(Restaurant.Description) => request.SortDirection switch
            {
                SortDirection.Ascending => q => q.OrderBy(r => r.Description),
                SortDirection.Descending => q => q.OrderByDescending(r => r.Description),
                _ => null
            },
            nameof(Restaurant.CategoryId) => request.SortDirection switch
            {
                SortDirection.Ascending => q => q.OrderBy(r => r.CategoryId),
                SortDirection.Descending => q => q.OrderByDescending(r => r.CategoryId),
                _ => null
            },

            _ => null
        };



        var restaurants = await _unitOfWork.RestaurantRepository.GetAllAsync(
            filter: filter,
            pageSize: request.PageSize,
            pageNumber: request.PageNumber,
            orderBy: orderBy
            );


        var restaurantDtos = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        var totalCount = await _unitOfWork.RestaurantRepository.CountAsync(filter);

        var result = new PageResult<RestaurantDto>(restaurantDtos, totalCount, request.PageSize, request.PageNumber);

        return result;
    }
}