using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Api.Models.Reservation.Admin;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Services;
using SittingModel = ReservationSystem_Server.Areas.Api.Models.Sitting.SittingModel;

namespace ReservationSystem_Server.Areas.Api.Controllers.Admin;

[ApiController]
[Area("Api")]
[Authorize(Roles = "Employee")]
[Route("api/v1/admin/[controller]")]
public class ReservationController : Controller
{
    private readonly ReservationUtility _reservationUtility;
    private readonly SittingUtility _sittingUtility;
    private readonly CustomerManager _customerManager;

    public ReservationController(ReservationUtility reservationUtility, SittingUtility sittingUtility,
        CustomerManager customerManager)
    {
        _reservationUtility = reservationUtility;
        _sittingUtility = sittingUtility;
        _customerManager = customerManager;
    }

    // TODO: move both get and get by id to /api/v1/admin/sittings
    /// <summary>
    /// Get all sittings based on given criteria
    /// </summary>
    /// <param name="includePast">Whether to include past sittings</param>
    /// <param name="includeClosed">Whether to include closed sittings</param>
    /// <response code="200">All sittings that match criteria</response>
    [HttpGet]
    [ProducesResponseType(typeof(SittingModel[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(bool includePast = false, bool includeClosed = false)
    {
        SittingModel[] sittings = (await _sittingUtility.GetSittingsAsync(includePast, includeClosed))
            .Select(s => new SittingModel().FromSitting(s)).ToArray();

        return Ok(sittings);
    }

    /// <summary>
    /// Get a sitting by id
    /// </summary>
    /// <param name="id">The id of the sitting to return</param>
    /// <response code="200">Returns the sitting</response>
    /// <response code="404">If the sitting does not exist</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SittingModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        Sitting? sitting = await _sittingUtility.GetSittingAsync(id);

        if (sitting == null)
        {
            return NotFound();
        }

        return Ok(new SittingModel().FromSitting(sitting));
    }

    /// <summary>
    /// Gets all reservations for a given sitting
    /// </summary>
    /// <param name="id">The id of the sitting</param>
    /// <response code="200">All reservations for the sitting</response>
    /// <response code="404">If the sitting does not exist</response>
    [HttpGet("list/{id:int}")]
    [ProducesResponseType(typeof(ReservationModel[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reservations(int id)
    {
        Sitting? sitting = await _sittingUtility.GetSittingAsync(id);
        
        if (sitting == null)
        {
            return NotFound();
        }

        Reservation[] reservations = await _reservationUtility.GetReservationsForSittingAsync(id, q => q
            .Include(r => r.Customer)
        );
        
        return Ok(reservations.Select(ReservationModel.FromReservation).ToArray());
    }
    
    /// <summary>
    /// Create a new reservation
    /// </summary>
    /// <param name="model">The data to create new reservation with</param>
    /// <response code="201">The created reservation</response>
    /// <response code="400">If the reservation is invalid</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(ReservationModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateModel model)
    {
        model.Validate(ModelState);

        Reservation reservation = new()
        {
            SittingId = model.SittingId,

            StartTime = model.StartTime,
            Duration = model.Duration,

            ReservationOriginId = model.ReservationOriginId,
            ReservationStatusId = 1,

            NumberOfPeople = model.NumberOfGuests,
            Notes = model.Notes
        };

        await _reservationUtility.ValidateReservationAsync(reservation, ModelState, true);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Customer customer = await _customerManager.GetOrCreateCustomerAsync(
            model.Customer.FirstName, model.Customer.LastName, model.Customer.Email, model.Customer.PhoneNumber);
        reservation.CustomerId = customer.Id;

        await _reservationUtility.CreateReservationAsync(reservation);

        return CreatedAtAction(nameof(Create), ReservationModel.FromReservation(reservation));
    }

    /// <summary>
    /// Update a new reservation
    /// </summary>
    /// <param name="id">The id of the reservation to update</param>
    /// <param name="model">The data to update a reservation with</param>
    /// <response code="200">The updated reservation</response>
    /// <response code="400">If the reservation is invalid</response>
    /// <response code="404">If the reservation does not exist</response>
    [HttpPut("{id:int}/edit")]
    [ProducesResponseType(typeof(ReservationModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(EmptyResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(int id, CreateModel model)
    {
        Reservation? reservation = await _reservationUtility.GetReservationAsync(id);

        if (reservation == null)
        {
            return NotFound();
        }

        if (reservation.SittingId != model.SittingId)
        {
            ModelState.AddModelError(nameof(model.SittingId), "Sitting does not match reservation");
        }

        model.Validate(ModelState);

        Reservation updated = new()
        {
            SittingId = model.SittingId,

            StartTime = model.StartTime,
            Duration = model.Duration,

            ReservationOriginId = model.ReservationOriginId,
            ReservationStatusId = 1,

            NumberOfPeople = model.NumberOfGuests,
            Notes = model.Notes
        };
        await _reservationUtility.ValidateReservationAsync(updated, ModelState, false);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        Customer customer = await _customerManager.GetOrCreateCustomerAsync(
            model.Customer.FirstName, model.Customer.LastName, model.Customer.Email, model.Customer.PhoneNumber);

        updated.CustomerId = customer.Id;
        await _reservationUtility.EditReservationAsync(updated);

        return Ok(ReservationModel.FromReservation(reservation));
    }

    /// <summary>
    /// Updates the status of a reservation
    /// </summary>
    /// <param name="id">The id of the reservation</param>
    /// <param name="statusId">The id of the new status</param>
    /// <returns>The updated reservation</returns>
    [HttpPost("{id:int}/status")]
    [ProducesResponseType(typeof(ReservationModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetStatus(int id, int statusId)
    {
        Reservation? reservation = await _reservationUtility.GetReservationAsync(id);
        ReservationStatus? status = await _reservationUtility.GetStatusAsync(statusId);
        
        if(reservation == null)
        {
            return NotFound();
        }
        
        if(status == null)
        {
            return BadRequest();
        }
        
        reservation.ReservationStatusId = statusId;
        await _reservationUtility.EditReservationAsync(reservation);

        return Ok(ReservationModel.FromReservation(reservation));
    }
    
    /// <summary>
    /// Retrieves all reservation origins
    /// </summary>
    /// <response code="200">Returns all reservation origins</response>
    [HttpGet("origins")]
    [ProducesResponseType(typeof(ReservationOrigin[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrigins()
    {
        return Ok(await _reservationUtility.GetOriginsAsync());
    }

    /// <summary>
    /// Retrieves reservation origin by id
    /// </summary>
    /// <param name="id">The id of the reservation origin to return</param>
    /// <response code="200">Returns the reservation origin</response>
    /// <response code="404">If the reservation origin does not exist</response>
    [HttpGet("origin/{id:int}")]
    [ProducesResponseType(typeof(ReservationOrigin), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrigin(int id)
    {
        ReservationOrigin? origin = await _reservationUtility.GetOriginAsync(id);
        if (origin == null)
        {
            return NotFound();
        }

        return Ok(origin);
    }

    /// <summary>
    /// Retrieves all reservation statuses
    /// </summary>
    /// <response code="200">Returns all reservation statuses</response>
    [HttpGet("statuses")]
    [ProducesResponseType(typeof(ReservationStatus[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatuses()
    {
        return Ok(await _reservationUtility.GetStatusesAsync());
    }

    /// <summary>
    /// Retrieves reservation status by id
    /// </summary>
    /// <param name="id">The id of the reservation status to return</param>
    /// <response code="200">Returns the reservation status</response>
    /// <response code="404">If the reservation status does not exist</response>
    [HttpGet("status/{id:int}")]
    [ProducesResponseType(typeof(ReservationStatus), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStatus(int id)
    {
        ReservationStatus? status = await _reservationUtility.GetStatusAsync(id);
        if (status == null)
        {
            return NotFound();
        }

        return Ok(status);
    }
}