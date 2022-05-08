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
    private readonly ApplicationDbContext _context;
    private readonly ReservationUtility _utility;
    private readonly SittingUtility _sittingUtility;
    private readonly CustomerManager _customerManager;

    public ReservationController(ApplicationDbContext context, ReservationUtility utility, SittingUtility sittingUtility, CustomerManager customerManager)
    {
        _context = context;
        _utility = utility;
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
        SittingModel[] sittings = await _sittingUtility.GetSittings(includePast, includeClosed)
            .Select(s => new SittingModel().FromSitting(s)).ToArrayAsync();

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
        if (await _sittingUtility.GetSittingAsync(model.SittingId) == null)
        {
            ModelState.AddModelError(nameof(model.SittingId), "Sitting does not exist");
        }

        if (await _utility.GetOriginAsync(model.ReservationOriginId) == null)
        {
            ModelState.AddModelError(nameof(model.ReservationOriginId), "Origin does not exist");
        }

        model.Validate(ModelState);
        
        if (ModelState.IsValid)
        {
            Customer customer = await _customerManager.GetOrCreateCustomerAsync(
                model.Customer.FirstName, model.Customer.LastName, model.Customer.Email, model.Customer.PhoneNumber);

            Reservation reservation = new()
            {
                SittingId = model.SittingId,
                CustomerId = customer.Id,
                
                StartTime = model.StartTime,
                Duration = model.Duration,
                
                ReservationOriginId = model.ReservationOriginId,
                ReservationStatusId = 1,
                
                NumberOfPeople = model.NumberOfGuests,
                Notes = model.Notes
            };
            
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), ReservationModel.FromReservation(reservation));
        }

        // If we get here, something went wrong
        return BadRequest(ModelState);
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
        if (await _sittingUtility.GetSittingAsync(model.SittingId) == null)
        {
            ModelState.AddModelError(nameof(model.SittingId), "Sitting does not exist");
        }

        if (await _utility.GetOriginAsync(model.ReservationOriginId) == null)
        {
            ModelState.AddModelError(nameof(model.ReservationOriginId), "Origin does not exist");
        }
        
        Reservation? reservation = await _context.Reservations.FindAsync(id);
        
        if(reservation == null)
        {
            return NotFound();
        }
        
        if(reservation.SittingId != model.SittingId)
        {
            ModelState.AddModelError(nameof(model.SittingId), "Sitting does not match reservation");
        }
        
        model.Validate(ModelState);

        if (ModelState.IsValid)
        {
            Customer customer = await _customerManager.GetOrCreateCustomerAsync(
                model.Customer.FirstName, model.Customer.LastName, model.Customer.Email, model.Customer.PhoneNumber);
            
            reservation.StartTime = model.StartTime;
            reservation.Duration = model.Duration;
            reservation.ReservationOriginId = model.ReservationOriginId;
            reservation.NumberOfPeople = model.NumberOfGuests;
            reservation.Notes = model.Notes;
            reservation.CustomerId = customer.Id;
            
            await _context.SaveChangesAsync();
            return Ok(ReservationModel.FromReservation(reservation));
        }
        
        // If we get here, something went wrong
        return BadRequest(ModelState);
    }
    
    /// <summary>
    /// Retrieves all reservation origins
    /// </summary>
    /// <response code="200">Returns all reservation origins</response>
    [HttpGet("origins")]
    [ProducesResponseType(typeof(ReservationOrigin[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrigins()
    {
        return Ok(await _utility.GetOriginsAsync());
    }
    
    /// <summary>
    /// Retrieves reservation origin by id
    /// </summary>
    /// <param name="id">The id of the reservation origin to return</param>
    /// <response code="200">Returns the reservation origin</response>
    /// <response code="404">If the reservation origin does not exist</response>
    [HttpGet("origins/{id:int}")]
    [ProducesResponseType(typeof(ReservationOrigin[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrigin(int id)
    {
        ReservationOrigin? origin = await _utility.GetOriginAsync(id);
        if (origin == null)
        {
            return NotFound();
        }

        return Ok(origin);
    }
}