using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> _logger,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    IRestaurantAuthorizationService _restaurantAuthorizationService) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating a restaurant Id => {requestId} with {@UpdatedRestaurant}", request.Id, request);

        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
        {
            _logger.LogWarning("Unauthorized attempt to update restaurant Id => {requestId}", request.Id);
            throw new ForbiddenException(nameof(Restaurant), restaurant.Id.ToString(), ResourceOperation.Update.ToString());
        }

        _mapper.Map(request, restaurant);

        _unitOfWork.RestaurantRepository.Update(restaurant);

        await _unitOfWork.CommitAsync();
    }
}