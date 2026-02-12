using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

public class RestaurantService(IUnitOfWork _unitOfWork,
    ILogger<RestaurantService> _logger,
    IMapper _mapper,
    IValidator<CreateRestaurantDto> _validator
    ) : IRestaurantService
{
    public async Task<IEnumerable<RestaurantDto?>> GetAllAsync()
    {
        _logger.LogInformation("Getting all restaurants");
        var restaurants = await _unitOfWork.RestaurantRepository.GetAllAsync();

        //return restaurants.Select(RestaurantDto.FromEntity);
        return _mapper.Map<IEnumerable<RestaurantDto?>>(restaurants);
    }
    public async Task<RestaurantDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Get Restaurant of Id => {{{id}}}");
        var restaurant = await _unitOfWork.RestaurantRepository.GetById(id);
        //return RestaurantDto.FromEntity(restaurant);
        return _mapper.Map<RestaurantDto?>(restaurant);
    }
    public async Task<int> Create(CreateRestaurantDto dto)
    {
        var result = await _validator.ValidateAsync(dto);

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        _logger.LogInformation("Creating a new restaurant");

        var restaurant = _mapper.Map<Restaurant>(dto);

        await _unitOfWork.RestaurantRepository.CreateAsync(restaurant);

        await _unitOfWork.CommitAsync();

        return restaurant.Id;
    }
}
