using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Admin.Models.Reservation;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Helper;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Employee")]
public class ReservationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly CustomerManager _customerManager;
    private readonly ReservationUtility _reservationUtility;
    private readonly SittingUtility _sittingUtility;
    
    private readonly ILogger<ReservationController> _logger;

    public ReservationController(ApplicationDbContext context, CustomerManager customerManager,
        ReservationUtility reservationUtility, SittingUtility sittingUtility, ILogger<ReservationController> logger)
    {
        _context = context;
        _customerManager = customerManager;
        _reservationUtility = reservationUtility;
        _sittingUtility = sittingUtility;
        _logger = logger;
    }

    public async Task<IActionResult> Index(bool pastSittings)
    {
        _logger.LogTrace("Entering GET Index");
        
        _logger.LogTrace("Setting past sittings view data to {PastSittings}", pastSittings);
        ViewData["PastSittings"] = pastSittings;
        
        _logger.LogTrace("Exiting GET Index");
        return View(await _sittingUtility.GetSittingsAsync(pastSittings, true, q => q
            .Include(s => s.SittingType)
            .Include(s => s.Reservations)));
    }

    public async Task<IActionResult> Sitting(int id)
    {
        _logger.LogTrace("Entering GET Sitting with id {Id}", id);
        
        Sitting? sitting = await _sittingUtility.GetSittingAsync(id, q => q
            .Include(s => s.SittingType)
            .Include(s => s.Reservations).ThenInclude(r => r.ReservationOrigin)
            .Include(s => s.Reservations).ThenInclude(r => r.ReservationStatus)
            .Include(s => s.Reservations).ThenInclude(r => r.Customer)
            .Include(s => s.Reservations).ThenInclude(r => r.Tables));

        if (sitting == null)
        {
            _logger.LogInformation("Sitting with id {Id} not found", id);
            return NotFound();
        }

        _logger.LogTrace("Exiting GET Sitting");
        return View(sitting);
    }

    public async Task<IActionResult> Details(int id)
    {
        _logger.LogTrace("Entering GET Details with id {Id}", id);

        Reservation? reservation = await _reservationUtility.GetReservationAsync(id, q => q
            .Include(r => r.ReservationOrigin)
            .Include(r => r.ReservationStatus)
            .Include(r => r.Sitting).ThenInclude(s => s.SittingType)
            .Include(r => r.Customer)
            .Include(r => r.Tables));

        if (reservation == null)
        {
            _logger.LogInformation("Reservation with id {Id} not found", id);
            return NotFound();
        }

        _logger.LogTrace("Exiting GET Details");
        return View(reservation);
    }

    public async Task<IActionResult> Create(int sittingId)
    {
        _logger.LogTrace("Entering GET Create with sitting id {SittingId}", sittingId);
        Sitting? sitting = await _sittingUtility.GetSittingAsync(sittingId);

        if (sitting == null)
        {
            _logger.LogInformation("Sitting with id {SittingId} not found", sittingId);
            return NotFound();
        }

        CreateViewModel model = new()
        {
            SittingId = sitting.Id,
            SittingStart = sitting.StartTime,
            SittingEnd = sitting.EndTime,

            StartTime = sitting.StartTime,
            Duration = sitting.DefaultDuration,
            DefaultDuration = sitting.DefaultDuration,

            AvailableOrigins = await _reservationUtility.GetOriginsAsSelectListAsync(),
            TimeSlots = _reservationUtility.GetTimeSlots(sitting.StartTime, sitting.EndTime, sitting.DefaultDuration)
        };

        _logger.LogTrace("Exiting GET Create with model {@Model}", model);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateViewModel model)
    {
        _logger.LogTrace("Entering POST Create with model {@Model}", model);
        if (string.IsNullOrWhiteSpace(model.Email) && string.IsNullOrWhiteSpace(model.Phone))
        {
            _logger.LogInformation("Email and phone are both empty, adding model error");
            ModelState.AddModelError("", "Email or phone number is required");
        }

        Reservation reservation = new()
        {
            SittingId = model.SittingId,
            StartTime = model.StartTime,
            Duration = model.Duration,
            NumberOfPeople = model.NumGuests,
            ReservationOriginId = model.Origin,
            Notes = model.Notes,
            ReservationStatusId = 1
        };

        _logger.LogTrace("Validating reservation {@Reservation}", reservation);
        await _reservationUtility.ValidateReservationAsync(reservation, ModelState, false, true);
        
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Validation failed for reservation {@Reservation}", reservation);
            
            model.AvailableOrigins = await _reservationUtility.GetOriginsAsSelectListAsync();
            model.TimeSlots = _reservationUtility.GetTimeSlots(model.SittingStart, model.SittingEnd, model.DefaultDuration);
            return View(model);
        }
        
        _logger.LogTrace("Validation succeeded");
        
        Customer customer = await _customerManager.GetOrCreateCustomerAsync(model.FirstName, model.LastName,
            model.Email, model.Phone);
        
        reservation.CustomerId = customer.Id;

        _logger.LogInformation("Creating reservation {@Reservation}", reservation);
        await _reservationUtility.CreateReservationAsync(reservation);

        _logger.LogTrace("Exiting POST Create");
        return RedirectToAction(nameof(Confirmation), new { id = reservation.Id });
    }

    public async Task<IActionResult> Edit(int id)
    {
        _logger.LogTrace("Entering GET Edit with reservation id {Id}", id);
        Reservation? reservation = await _reservationUtility.GetReservationAsync(id, q => q
            .Include(r => r.Sitting)
            .Include(r => r.ReservationOrigin)
            .Include(c => c.Customer));

        if (reservation == null)
        {
            _logger.LogInformation("Reservation {Id} not found", id);
            return NotFound();
        }

        EditViewModel model = new()
        {
            SittingId = reservation.SittingId,
            ReservationId = reservation.Id,

            SittingStart = reservation.Sitting.StartTime,
            SittingEnd = reservation.Sitting.EndTime,
            StartTime = reservation.StartTime,
            Duration = reservation.Duration,
            NumGuests = reservation.NumberOfPeople,
            Origin = reservation.ReservationOriginId,

            FirstName = reservation.Customer.FirstName,
            LastName = reservation.Customer.LastName,
            Email = reservation.Customer.Email,
            Phone = reservation.Customer.PhoneNumber,

            Notes = reservation.Notes,

            AvailableOrigins = await _reservationUtility.GetOriginsAsSelectListAsync(),
            TimeSlots = _reservationUtility.GetTimeSlots(reservation.Sitting.StartTime, reservation.Sitting.EndTime,
                reservation.Sitting.DefaultDuration)
        };

        _logger.LogTrace("Exit GET Edit with model {@Model}", model);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditViewModel model)
    {
        _logger.LogTrace("Entering POST Edit with model {@Model}", model);
        if (string.IsNullOrWhiteSpace(model.Email) && string.IsNullOrWhiteSpace(model.Phone))
        {
            _logger.LogInformation("Email and phone are both empty, adding model error");
            ModelState.AddModelError("", "Email or phone number is required");
        }

        Sitting? sitting = await _sittingUtility.GetSittingAsync(model.SittingId);
        if (sitting == null)
        {
            _logger.LogWarning("Sitting {Id} not found when editing existing reservation", model.SittingId);
            return NotFound(); // Irrecoverable state as valid sitting required to calculate time slots
        }

        Reservation? reservation = await _reservationUtility.GetReservationAsync(model.ReservationId, q => q
            .Include(r => r.Sitting)
            .Include(r => r.ReservationOrigin));

        if (reservation == null)
        {
            _logger.LogInformation("Attempted to edit reservation {Id} that does not exist", model.ReservationId);
            return NotFound();
        }

        Reservation updated = new()
        {
            Id = model.ReservationId,
            SittingId = model.SittingId,
            StartTime = model.StartTime,
            Duration = model.Duration,
            NumberOfPeople = model.NumGuests,
            ReservationOriginId = model.Origin,
            Notes = model.Notes,
            ReservationStatusId = 1
        };

        _logger.LogTrace("Validating reservation {@Reservation}", updated);
        await _reservationUtility.ValidateReservationAsync(updated, ModelState, false, true);
        
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Validation failed for reservation {@Reservation}", updated);
            model.AvailableOrigins = await _reservationUtility.GetOriginsAsSelectListAsync();
            model.TimeSlots = _reservationUtility.GetTimeSlots(sitting.StartTime, sitting.EndTime, sitting.DefaultDuration);
            return View(model);
        }

        Customer customer =
            await _customerManager.GetOrCreateCustomerAsync(model.FirstName, model.LastName, model.Email, model.Phone);
        updated.CustomerId = customer.Id;

        _logger.LogTrace("Updating reservation {@Reservation}", updated);
        await _reservationUtility.EditReservationAsync(updated);

        _logger.LogTrace("Exit POST Edit");
        return RedirectToAction(nameof(Confirmation), new { id = updated.Id, edit = true });
    }

    public async Task<IActionResult> Confirmation(int id, bool? edit)
    {
        _logger.LogTrace("Entering GET Confirmation with id {Id} and edit flag {Edit}", id, edit);
        
        Reservation? reservation = await _reservationUtility.GetReservationAsync(id, q => q
            .Include(r => r.Sitting)
            .Include(r => r.ReservationOrigin)
            .Include(r => r.Sitting).ThenInclude(s => s.SittingType));

        if (reservation == null)
        {
            _logger.LogInformation("Reservation {Id} not found when showing confirmation page", id);
            return NotFound();
        }

        ViewBag.IsEdit = edit is true;

        _logger.LogTrace("Exit GET Confirmation");
        return View(reservation);
    }

    public async Task<IActionResult> UpdateStatus(int id)
    {
        _logger.LogTrace("Entering GET UpdateStatus with id {Id}", id);
        
        Reservation? reservation = await _reservationUtility.GetReservationAsync(id, q => q
            .Include(r => r.Customer));

        if (reservation == null)
        {
            _logger.LogInformation("Reservation {Id} not found when showing update status page", id);
            return NotFound();
        }

        UpdateStatusViewModel model = new()
        {
            ReservationId = id,
            StatusId = reservation.ReservationStatusId,
            ReservationDetails =
                $"{reservation.Customer.FirstName} {reservation.Customer.LastName} at {reservation.StartTime.ToShortTimeString()}",
            Statuses = new SelectList(await _reservationUtility.GetStatusesAsync(),
                nameof(ReservationStatus.Id), nameof(ReservationStatus.Description))
        };

        _logger.LogTrace("Exit GET UpdateStatus with model {@Model}", model);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(UpdateStatusViewModel model)
    {
        _logger.LogTrace("Entering POST UpdateStatus with model {@Model}", model);
        
        if(await _reservationUtility.GetStatusAsync(model.StatusId) == null)
            ModelState.AddModelError(nameof(UpdateStatusViewModel.StatusId), "Could not find status with given id");
        
        if (!ModelState.IsValid)
            return View(model);

        Reservation? reservation = await _reservationUtility.GetReservationAsync(model.ReservationId);
        if (reservation == null)
        {
            _logger.LogInformation("Reservation {Id} not found when updating status", model.ReservationId);
            return NotFound();
        }

        _logger.LogTrace("Updating reservation {@Reservation}", reservation);
        reservation.ReservationStatusId = model.StatusId;
        await _reservationUtility.EditReservationAsync(reservation);

        _logger.LogTrace("Exiting POST UpdateStatus");
        return this.CloseModalAndRefresh();
    }

    public async Task<IActionResult> AssignTables(int id)
    {
        _logger.LogTrace("Entering GET AssignTables with id {Id}", id);
        
        Reservation? reservation = await _reservationUtility.GetReservationAsync(id, q => q
            .Include(r => r.Sitting).ThenInclude(s => s.Restaurant)
            .ThenInclude(r => r.Areas).ThenInclude(a => a.Tables)
            .Include(r => r.Tables));

        if (reservation == null)
        {
            _logger.LogInformation("Reservation {Id} not found when showing assign tables page", id);
            return NotFound();
        }

        AssignTablesViewModel model = new()
        {
            ReservationId = id,
            SittingId = reservation.SittingId,
            Tables = reservation.Sitting.Restaurant.Areas.SelectMany(a => a.Tables).OrderBy(t => t.Id)
                .Select(t => new AssignTablesViewModel.Table
                {
                    Id = t.Id,
                    Name = t.Name,
                    IsAssigned = reservation.Tables.Any(rt => rt.Id == t.Id)
                }).ToList()
        };

        _logger.LogTrace("Exit GET AssignTables with model {@Model}", model);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignTables(AssignTablesViewModel model)
    {
        _logger.LogTrace("Entering POST AssignTables with model {@Model}", model);

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Validation failed for model {@Model}", model);
            return View(model);
        }

        Reservation? reservation = await _reservationUtility.GetReservationAsync(model.ReservationId, q => q
            .Include(r => r.Tables));

        if (reservation == null)
        {
            _logger.LogInformation("Reservation {Id} not found when assigning tables", model.ReservationId);
            return NotFound();
        }

        _logger.LogTrace("Updating reservation {@Reservation}", reservation);
        reservation.Tables.Clear();
        reservation.Tables.AddRange(model.Tables
            .Where(t => t.IsAssigned)
            .Select(t => _context.Tables.First(tbl => tbl.Id == t.Id)));

        await _reservationUtility.EditReservationAsync(reservation);
        
        _logger.LogTrace("Exiting POST AssignTables");
        return RedirectToAction(nameof(Sitting), new { id = reservation.SittingId });
    }
}