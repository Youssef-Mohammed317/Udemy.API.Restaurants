using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(IUnitOfWork _unitOfWork,
    ILogger<DeleteRestaurantCommandHandler> _logger,
    IRestaurantAuthorizationService _restaurantAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {

        _logger.LogInformation("Delete restaurant Id => {requestId}", request.Id);

        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(request.Id)
             ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
        {
            _logger.LogWarning("Unauthorized attempt to delete restaurant Id => {requestId}", request.Id);
            throw new ForbiddenException(nameof(Restaurant), restaurant.Id.ToString(), ResourceOperation.Delete.ToString());
        }


        _unitOfWork.RestaurantRepository.Delete(restaurant);

        await _unitOfWork.CommitAsync();


    }
}