using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllRestaurantDishes;

public class DeleteAllRestaurantDishesCommandHandler(IUnitOfWork _unitOfWork,
    ILogger<DeleteAllRestaurantDishesCommandHandler> _logger) : IRequestHandler<DeleteAllRestaurantDishesCommand>
{
    public async Task Handle(DeleteAllRestaurantDishesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting all dishes for restaurant Id:{restaurantId}", request.RestaurantId);

        var restaurantExists = await _unitOfWork.RestaurantRepository.CheckExistAsync(r => r.Id == request.RestaurantId);

        if (!restaurantExists)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        await _unitOfWork.DishRepository.DeleteAllAsync(d => d.RestaurantId == request.RestaurantId); // use ExecuteDeleteAsync, dont need to commit changes


    }
}