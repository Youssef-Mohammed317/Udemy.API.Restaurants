using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(IUnitOfWork _unitOfWork,
    ILogger<DeleteRestaurantCommandHandler> _logger) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {

        _logger.LogInformation("Delete restaurant Id => {requestId}", request.Id);

        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(request.Id)
             ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());


        _unitOfWork.RestaurantRepository.Delete(restaurant);

         await _unitOfWork.CommitAsync();


    }
}