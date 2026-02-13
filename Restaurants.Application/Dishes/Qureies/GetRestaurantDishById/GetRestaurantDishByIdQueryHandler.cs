using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Qureies.GetRestaurantDishById;

public class GetRestaurantDishByIdQueryHandler(IUnitOfWork _unitOfWork,
    IMapper _mapper,
    ILogger<GetRestaurantDishByIdQueryHandler> _logger)
    : IRequestHandler<GetRestaurantDishByIdQuery, DishDto>
{
    public async Task<DishDto> Handle(GetRestaurantDishByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get Dish id : {requestId} for restaurant {requestRestaurantId}", request.Id, request.RestaurantId);

        var restaurantExists = await _unitOfWork.RestaurantRepository.CheckExistAsync(
        filter: r => r.Id == request.RestaurantId);
        if (!restaurantExists)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = await _unitOfWork.DishRepository.GetFirstAsync(filter:
            d => d.Id == request.Id && d.RestaurantId == request.RestaurantId)
            ?? throw new NotFoundException(nameof(Dish), request.Id.ToString());

        var dto = _mapper.Map<DishDto>(dish);

        return dto;
    }
}