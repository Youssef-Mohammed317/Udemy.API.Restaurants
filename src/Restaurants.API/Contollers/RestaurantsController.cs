using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Contollers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RestaurantsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    //[Authorize(Policy = PolicyNames.AtLeastOwnerOf2Restaurants)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query)
    {
        var restaurants = await _mediator.Send(query);
        return Ok(restaurants);
    }
    [HttpGet("{id:int}")]
    //[Authorize(Policy = PolicyNames.HasNationality)]
    public async Task<ActionResult<RestaurantDto>> GetById([FromRoute] int id)
    {
        var restaurant = await _mediator.Send(new GetRestaurantByIdQuery(id));
        return Ok(restaurant);
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Authorize(Roles = UserRoles.Owner)]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);

    }
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _mediator.Send(new DeleteRestaurantCommand(id));
        return NoContent();
    }
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRestaurantCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);

        return NoContent();

    }

    [HttpPost("{id}/logo")]
    public async Task<IActionResult> UploadLogo([FromRoute] int id, IFormFile logo)
    {
        if (logo == null || logo.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        using var stream = logo.OpenReadStream();

        var command = new UploadRestaurantLogoCommand
        {
            RestaurantId = id,
            FileName = $"{id}-{logo.FileName}",
            File = stream
        };
        await _mediator.Send(command);
        return NoContent();
    }
}
