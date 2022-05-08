using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Api.Models.Sitting;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Areas.Api.Controllers;

[ApiController]
[Route(ApiInfo.Endpoint + "[controller]")]
public class SittingsController : Controller
{
    private readonly SittingUtility _utility;
    private readonly ReservationUtility _reservationUtility;
    
    //TODO configurable from sitting
    private static readonly TimeSpan TimeSlotLength = TimeSpan.FromMinutes(30);

    public SittingsController(SittingUtility utility, ReservationUtility reservationUtility)
    {
        _utility = utility;
        _reservationUtility = reservationUtility;
    }

    /// <summary>
    /// Get all available sittings
    /// </summary>
    /// <remarks>
    /// Available sittings are sittings that are not closed and are in the future,
    /// Getting past or closed sittings can only be done using the admin endpoint
    /// </remarks>
    /// <response code="200">All available sittings</response>
    [HttpGet]
    [ProducesResponseType(typeof(SittingModel[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        SittingModel[] sittings = await _utility.GetSittings().Select(s => new SittingModel().FromSitting(s))
            .ToArrayAsync();

        return Ok(sittings);
    }
    
    /// <summary>
    /// Get an available sitting by id
    /// </summary>
    /// <remarks>
    /// Available sittings are sittings that are not closed and are in the future,
    /// Getting past or closed sittings can only be done using the admin endpoint
    /// </remarks>
    /// <param name="id">The id of the sitting to return</param>
    /// <response code="200">Returns the sitting</response>
    /// <response code="404">If the sitting does not exist or is unavailable</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SittingModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        Sitting? sitting = await _utility.GetSittingAsync(id);

        if (sitting == null || !IsAvailable(sitting))
        {
            return NotFound();
        }
        
        return Ok(new SittingModel().FromSitting(sitting));
    }
    
    /// <summary>
    /// Gets a list of time slots for a given available sitting
    /// </summary>
    /// <remarks>
    /// Available sittings are sittings that are not closed and are in the future,
    /// Getting past or closed sittings can only be done using the admin endpoint
    /// </remarks>
    /// <param name="id">The id of the sitting to calculate time slots for</param>
    /// <response code="200">Returns the time slots</response>
    /// <response code="404">If the sitting does not exist or is unavailable</response>
    [HttpGet("timeSlots/{id:int}")]
    [ProducesResponseType(typeof(List<DateTime>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTimeSlots(int id)
    {
        Sitting? sitting = await _utility.GetSittingAsync(id);

        if (sitting == null || !IsAvailable(sitting))
        {
            return NotFound();
        }

        return Ok(_reservationUtility.GetTimeSlots(sitting.StartTime, sitting.EndTime, TimeSlotLength));
    }
    
    private static bool IsAvailable(Sitting sitting)
    {
        return !sitting.IsClosed && sitting.EndTime > DateTime.Now;
    }
}