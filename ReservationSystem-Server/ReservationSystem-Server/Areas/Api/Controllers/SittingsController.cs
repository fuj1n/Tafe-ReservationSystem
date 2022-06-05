using Microsoft.AspNetCore.Mvc;
using ReservationSystem_Server.Areas.Api.Models.Sitting;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Areas.Api.Controllers;

[ApiController]
[Route(ApiInfo.Base + "[controller]")]
public class SittingsController : Controller
{
    private readonly SittingUtility _sittingUtility;
    private readonly ReservationUtility _reservationUtility;

    public SittingsController(SittingUtility sittingUtility, ReservationUtility reservationUtility)
    {
        _sittingUtility = sittingUtility;
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
        SittingModel[] sittings =
            (await _sittingUtility.GetSittingsAsync()).Select(s => new SittingModel().FromSitting(s)).ToArray();

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
        Sitting? sitting = await _sittingUtility.GetSittingAsync(id);

        if (sitting == null || !_sittingUtility.EvaluateAvailability(sitting))
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
        Sitting? sitting = await _sittingUtility.GetSittingAsync(id);

        if (sitting == null || !_sittingUtility.EvaluateAvailability(sitting))
        {
            return NotFound();
        }

        return Ok(_reservationUtility.GetTimeSlots(sitting.StartTime, sitting.EndTime, sitting.DefaultDuration));
    }

    [HttpGet("sittingTypes")]
    [ProducesResponseType(typeof(SittingType[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSittingTypes()
    {
        return Ok(await _sittingUtility.GetSittingTypesAsync());
    }
}