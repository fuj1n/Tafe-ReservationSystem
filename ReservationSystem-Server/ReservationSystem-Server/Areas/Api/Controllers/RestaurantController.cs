using Microsoft.AspNetCore.Mvc;
using ReservationSystem_Server.Areas.Api.Models.Restaurant;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Areas.Api.Controllers;

[ApiController]
[Route($"{ApiInfo.Base}[controller]")]
public class RestaurantController : ControllerBase
{
    private readonly RestaurantProvider _restaurantProvider;

    public RestaurantController(RestaurantProvider restaurantProvider)
    {
        _restaurantProvider = restaurantProvider;
    }
    
    /// <summary>
    /// Get the restaurant information for restaurant
    /// </summary>
    /// <response code="200">Returns the restaurant information</response>
    /// <response code="404">Restaurant not found</response>
    [HttpGet]
    [ProducesResponseType(typeof(RestaurantModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        Restaurant? restaurant = await _restaurantProvider.GetRestaurantDataAsync();

        if (restaurant == null)
        {
            return NotFound();
        }
        
        return Ok(new RestaurantModel
        {
            Name = restaurant.Name,
            Address = restaurant.Address,
            PhoneNumber = restaurant.PhoneNumber,
            Email = restaurant.Email
        });
    }
}