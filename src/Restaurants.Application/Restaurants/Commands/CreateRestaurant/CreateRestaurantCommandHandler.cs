using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> _logger,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    IUserContext _userContext) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {

        var user = _userContext.GetCurrentUser()
            ?? throw new InvalidOperationException("User context is not available");

        _logger.LogInformation("{UserEmail} [{UserId}] is creating a new restaurant {@restaurant}",
            user.Email, user.Id,
            request);

        var restaurant = _mapper.Map<Restaurant>(request);

        restaurant.OwnerId = user.Id;

        await _unitOfWork.RestaurantRepository.CreateAsync(restaurant);

        await _unitOfWork.CommitAsync();

        return restaurant.Id;
    }
}