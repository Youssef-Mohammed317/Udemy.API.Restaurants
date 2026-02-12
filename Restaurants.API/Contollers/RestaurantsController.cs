using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Services;

namespace Restaurants.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantsController(IServiceManager _serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await _serviceManager.RestaurantService.GetAllAsync();
        return Ok(restaurants);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var restaurant = await _serviceManager.RestaurantService.GetByIdAsync(id);
        return restaurant != null ? Ok(restaurant) : NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantDto dto)
    {
 
        var id = await _serviceManager.RestaurantService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id }, null);

    }
}
