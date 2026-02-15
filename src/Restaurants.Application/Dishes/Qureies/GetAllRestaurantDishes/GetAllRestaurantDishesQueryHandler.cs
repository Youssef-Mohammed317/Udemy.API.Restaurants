using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Qureies.GetAllRestaurantDishes;

public class GetAllRestaurantDishesQueryHandler(IUnitOfWork _unitOfWork,
    IMapper _mapper,
    ILogger<GetAllRestaurantDishesQueryHandler> _logger) : IRequestHandler<GetAllRestaurantDishesQuery, IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetAllRestaurantDishesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all dishes of restaurant id:{requestRestaurantId}", request.RestaurantId);

        var restaurantExists = await _unitOfWork.RestaurantRepository.CheckExistAsync(
                filter: r => r.Id == request.RestaurantId);
        if (!restaurantExists)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dishes = await _unitOfWork.DishRepository.GetAllAsync(
            filter: d => d.RestaurantId == request.RestaurantId
            );

        var dtos = _mapper.Map<IEnumerable<DishDto>>(dishes);

        return dtos;

    }
}