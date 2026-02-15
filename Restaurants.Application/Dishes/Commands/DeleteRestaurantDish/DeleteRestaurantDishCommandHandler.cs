using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Commands.UpdateRestaurantDish;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteRestaurantDish;

public class DeleteRestaurantDishCommandHandler(IUnitOfWork _unitOfWork,
    ILogger<DeleteRestaurantDishCommandHandler> _logger,
    IRestaurantAuthorizationService _restaurantAuthorizationService
    ) : IRequestHandler<DeleteRestaurantDishCommand>
{
    public async Task Handle(DeleteRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete dish Id {DishId} at restaurant {RestauarntId}", request.Id, request.RestaurantId);

        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(request.RestaurantId)
            ??
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());


        var canDelete = _restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete);
        if (!canDelete)
            throw new ForbiddenException(nameof(Restaurant), request.RestaurantId.ToString(), "Update");

        var dish = await _unitOfWork.DishRepository.GetByIdAsync(request.Id)
            ??
            throw new NotFoundException(nameof(Dish), request.Id.ToString());

        if (dish.RestaurantId != request.RestaurantId)
            throw new ForbiddenException(nameof(Dish), request.Id.ToString(), "Delete");



        _unitOfWork.DishRepository.Delete(dish);

        await _unitOfWork.CommitAsync();


    }
}