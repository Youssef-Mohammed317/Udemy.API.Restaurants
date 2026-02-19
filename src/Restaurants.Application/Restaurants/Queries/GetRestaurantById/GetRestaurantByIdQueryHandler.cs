using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(IUnitOfWork _unitOfWork,
    ILogger<GetRestaurantByIdQueryHandler> _logger,
    IMapper _mapper,
    IBlobStorageService _blobStorageService) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get Restaurant of Id => {requestId}", request.Id);

        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        var dto = _mapper.Map<RestaurantDto>(restaurant);
        dto.LogoSasUrl = _blobStorageService.GetBlobSasUrl(restaurant.LogoUrl);
        return dto;
    }
}

