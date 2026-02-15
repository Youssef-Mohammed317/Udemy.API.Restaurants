using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateRestaurantDish;

public class CreateRestaurantDishCommandHandler(IUnitOfWork _unitOfWork,
    IMapper _mapper,
    ILogger<CreateRestaurantDishCommandHandler> _logger,
    IRestaurantAuthorizationService _restaurantAuthorizationService) : IRequestHandler<CreateRestaurantDishCommand, int>
{
    public async Task<int> Handle(CreateRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Create new dish: {@DishRequest}", request);

        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
        {
            _logger.LogWarning("Unauthorized attempt to create dish for restaurant Id: {RestaurantId}", request.RestaurantId);
            throw new ForbiddenException(nameof(Restaurant), request.RestaurantId.ToString(), ResourceOperation.Update.ToString());
        }


        var dish = _mapper.Map<Dish>(request);

        await _unitOfWork.DishRepository.CreateAsync(dish);

        await _unitOfWork.CommitAsync();

        return dish.Id;
    }
}