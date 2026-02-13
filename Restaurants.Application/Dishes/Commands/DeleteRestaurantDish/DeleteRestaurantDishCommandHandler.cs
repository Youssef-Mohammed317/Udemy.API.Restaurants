using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Commands.UpdateRestaurantDish;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteRestaurantDish;

public class DeleteRestaurantDishCommandHandler(IUnitOfWork _unitOfWork,
    ILogger<UpdateRestaurantDishCommandHandler> _logger) : IRequestHandler<DeleteRestaurantDishCommand>
{
    public async Task Handle(DeleteRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete dish Id {DishId} at restaurant {RestauarntId}", request.Id, request.RestaurantId);

        var restaurantExists = await _unitOfWork.RestaurantRepository.CheckExistAsync(r => r.Id == request.RestaurantId);

        if (!restaurantExists)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = await _unitOfWork.DishRepository.GetByIdAsync(request.Id)
            ??
            throw new NotFoundException(nameof(Dish), request.Id.ToString());

        if (dish.RestaurantId != request.RestaurantId)
            throw new ForbiddenException(nameof(Dish), request.Id.ToString(), "Update");

        _unitOfWork.DishRepository.Delete(dish);

        await _unitOfWork.CommitAsync();


    }
}