using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommand : IRequest
{
    public int RestaurantId { get; set; }
    public Stream File { get; set; } = default!;
    public string FileName { get; set; } = default!;
}
public class UploadRestaurantLogoCommandHandler(ILogger<UploadRestaurantLogoCommandHandler> _logger,
    IUnitOfWork _unitOfWork,
    IRestaurantAuthorizationService _restaurantAuthorizationService,
    IBlobStorageService blobStorageService
    ) : IRequestHandler<UploadRestaurantLogoCommand>
{

    public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Upload restaurant logo for id : {restId}", request.RestaurantId);
        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbiddenException(nameof(Restaurant), request.RestaurantId.ToString(), ResourceOperation.Update.ToString());


        var logoUrl = await blobStorageService.UploadToBlobAsync(request.File, request.FileName);

        restaurant.LogoUrl = logoUrl;
        _unitOfWork.RestaurantRepository.Update(restaurant);
        await _unitOfWork.CommitAsync();
    }
}