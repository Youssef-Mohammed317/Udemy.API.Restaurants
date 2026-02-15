using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateRestaurantDish;
using Restaurants.Application.Dishes.Commands.DeleteAllRestaurantDishes;
using Restaurants.Application.Dishes.Commands.DeleteRestaurantDish;
using Restaurants.Application.Dishes.Commands.UpdateRestaurantDish;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Qureies.GetAllRestaurantDishes;
using Restaurants.Application.Dishes.Qureies.GetRestaurantDishById;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Contollers;

[ApiController]
[Route("api/restaurants/{restaurantId:int}/[controller]")]
[Authorize]
public class DishesController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAll([FromRoute] int restaurantId)
    {
        var dishes = await _mediator.Send(new GetAllRestaurantDishesQuery(restaurantId));
        return Ok(dishes);
    }
    [HttpGet("{id:int}")]
    [Authorize(Policy = PolicyNames.AtLeast20)]
    public async Task<ActionResult<DishDto>> GetById([FromRoute] int restaurantId, [FromRoute] int id)
    {
        var dish = await _mediator.Send(new GetRestaurantDishByIdQuery(restaurantId, id));
        return Ok(dish);
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromRoute] int restaurantId, [FromBody] CreateRestaurantDishCommand command)
    {
        command.RestaurantId = restaurantId;
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { restaurantId, id }, null);
    }
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int restaurantId, [FromRoute] int id, [FromBody] UpdateRestaurantDishCommand command)
    {
        command.Id = id;
        command.RestaurantId = restaurantId;
        await _mediator.Send(command);
        return NoContent();
    }
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById([FromRoute] int restaurantId, [FromRoute] int id)
    {
        await _mediator.Send(new DeleteRestaurantDishCommand(restaurantId, id));
        return NoContent();
    }
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAll([FromRoute] int restaurantId)
    {
        await _mediator.Send(new DeleteAllRestaurantDishesCommand(restaurantId));
        return NoContent();
    }
}
