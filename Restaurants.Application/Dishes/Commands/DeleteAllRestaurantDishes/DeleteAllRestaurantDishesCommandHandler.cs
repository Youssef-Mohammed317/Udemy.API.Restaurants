using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllRestaurantDishes;

public class DeleteAllRestaurantDishesCommandHandler(IUnitOfWork _unitOfWork,
    ILogger<DeleteAllRestaurantDishesCommandHandler> _logger,
    IRestaurantAuthorizationService _restaurantAuthorizationService) : IRequestHandler<DeleteAllRestaurantDishesCommand>
{
    public async Task Handle(DeleteAllRestaurantDishesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting all dishes for restaurant Id:{restaurantId}", request.RestaurantId);

        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(request.RestaurantId)
            ?? 
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        

        if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
        {
            _logger.LogWarning("Unauthorized attempt to delete the dishes of restaurant Id => {requestId}", request.RestaurantId);
            throw new ForbiddenException(nameof(Restaurant), request.RestaurantId.ToString(), ResourceOperation.Delete.ToString());
        }

        await _unitOfWork.DishRepository.DeleteAllAsync(d => d.RestaurantId == request.RestaurantId); // use ExecuteDeleteAsync, dont need to commit changes


    }
}