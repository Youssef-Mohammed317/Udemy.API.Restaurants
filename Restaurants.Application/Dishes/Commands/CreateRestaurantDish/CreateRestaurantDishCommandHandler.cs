using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateRestaurantDish;

public class CreateRestaurantDishCommandHandler(IUnitOfWork _unitOfWork,
    IMapper _mapper,
    ILogger<CreateRestaurantDishCommandHandler> _logger) : IRequestHandler<CreateRestaurantDishCommand, int>
{
    public async Task<int> Handle(CreateRestaurantDishCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Create new dish :{@DishRequest}", request);

        var restaurantExists = await _unitOfWork.RestaurantRepository.CheckExistAsync(
                r => r.Id == request.RestaurantId);

        if (!restaurantExists)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = _mapper.Map<Dish>(request);

        await _unitOfWork.DishRepository.CreateAsync(dish);

        await _unitOfWork.CommitAsync();

        return dish.Id;
    }
}