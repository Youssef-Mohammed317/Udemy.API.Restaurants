using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.UpdateRestaurantDish;

public class UpdateRestaurantDishCommandHandler(IUnitOfWork _unitOfWork,
    IMapper _mapper,
    ILogger<UpdateRestaurantDishCommandHandler> _logger) : IRequestHandler<UpdateRestaurantDishCommand>
{
    public async Task Handle(UpdateRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating a dish Id : {Id} at restaurant {RestaurantId} with {@UpdatedDish}", request.Id, request.RestaurantId, request);

        var restaurantExists = await _unitOfWork.RestaurantRepository.CheckExistAsync(r => r.Id == request.RestaurantId);

        if (!restaurantExists)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = await _unitOfWork.DishRepository.GetByIdAsync(request.Id)
            ??
            throw new NotFoundException(nameof(Dish), request.Id.ToString());

        if (dish.RestaurantId != request.RestaurantId)
            throw new ForbiddenException(nameof(Dish), request.Id.ToString(), "Update");

        _mapper.Map(request, dish);

        _unitOfWork.DishRepository.Update(dish);

        await _unitOfWork.CommitAsync();

    }

}