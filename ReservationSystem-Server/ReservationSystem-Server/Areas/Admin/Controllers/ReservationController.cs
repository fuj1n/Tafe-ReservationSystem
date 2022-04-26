using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Admin.Models.Reservation;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Employee")]
public class ReservationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly CustomerManager _customerManager;
    private readonly ReservationUtility _utility;
    
    private readonly TimeSpan _timeSlotLength = TimeSpan.FromMinutes(30);

    public ReservationController(ApplicationDbContext context, CustomerManager customerManager, ReservationUtility utility)
    {
        _context = context;
        _customerManager = customerManager;
        _utility = utility;
    }
    
    public async Task<IActionResult> Index()
    {
        // TODO: only future sittings
        return View(await _context.Sittings.Include(s => s.SittingType).ToListAsync());
    }

    public async Task<IActionResult> Sitting(int id)
    {
        Sitting? sitting = await _context.Sittings
            .Include(s => s.SittingType)
            .Include(s => s.Reservations).ThenInclude(r => r.ReservationOrigin)
            .Include(s => s.Reservations).ThenInclude(r => r.ReservationStatus)
            .Include(s => s.Reservations).ThenInclude(r => r.Customer)
            .Include(s => s.Reservations).ThenInclude(r => r.Tables)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (sitting == null)
        {
            return NotFound();
        }
        
        return View(sitting);
    }

    public async Task<IActionResult> Details(int id)
    {
        Reservation? reservation = await _context.Reservations
            .Include(r => r.ReservationOrigin)
            .Include(r => r.ReservationStatus)
            .Include(r => r.Sitting).ThenInclude(s => s.SittingType)
            .Include(r => r.Customer)
            .Include(r => r.Tables)
            .FirstOrDefaultAsync(r => r.Id == id);
        
        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
    }
    
    public async Task<IActionResult> Create(int sittingId)
    {
        Sitting? sitting = await _context.Sittings
            .FirstOrDefaultAsync(s => s.Id == sittingId);

        if (sitting == null)
        {
            return NotFound();
        }
        
        CreateViewModel model = new()
        {
            SittingId = sitting.Id,
            SittingStart = sitting.StartTime,
            SittingEnd = sitting.EndTime,
            
            StartTime = sitting.StartTime,
            
            AvailableOrigins = await GetAvailableOriginsAsync(),
            TimeSlots = _utility.GetTimeSlots(sitting.StartTime, sitting.EndTime, _timeSlotLength)
        };
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateViewModel model)
    {
        if(string.IsNullOrWhiteSpace(model.Email) && string.IsNullOrWhiteSpace(model.Phone))
        {
            ModelState.AddModelError("", "Email or phone number is required");
        }

        if (!ModelState.IsValid)
        {
            model.AvailableOrigins = await GetAvailableOriginsAsync();
            return View(model);
        }

        Sitting? sitting = await _context.Sittings
            .FirstOrDefaultAsync(s => s.Id == model.SittingId);

        if (sitting == null)
        {
            return NotFound();
        }

        Customer customer = await _customerManager.GetOrCreateCustomerAsync(model.FirstName, model.LastName,
                model.Email, model.Phone);
        
        Reservation reservation = new()
        {
            SittingId = model.SittingId,
            StartTime = model.StartTime,
            Duration = model.Duration,
            NumberOfPeople = model.NumGuests,
            ReservationOriginId = model.Origin,
            Notes = model.Notes,
            ReservationStatusId = 1,
            CustomerId = customer.Id
        };
        
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Sitting), new {id = reservation.SittingId, modal = ConfirmationUrl(reservation.Id)});
    }

    public async Task<IActionResult> Edit(int id)
    {
        Reservation? reservation = await _context.Reservations
            .Include(r => r.Sitting)
            .Include(r => r.ReservationOrigin)
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reservation == null)
            return NotFound();

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
            
            AvailableOrigins = await GetAvailableOriginsAsync(),
            TimeSlots = _utility.GetTimeSlots(reservation.Sitting.StartTime, reservation.Sitting.EndTime, _timeSlotLength)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditViewModel model)
    {
        if(string.IsNullOrWhiteSpace(model.Email) && string.IsNullOrWhiteSpace(model.Phone))
        {
            ModelState.AddModelError("", "Email or phone number is required");
        }

        Sitting? sitting = await _context.Sittings
            .FirstOrDefaultAsync(s => s.Id == model.SittingId);
        if(sitting == null)
            return NotFound();
        
        if(!ModelState.IsValid)
        {
            model.AvailableOrigins = await GetAvailableOriginsAsync();
            model.TimeSlots = _utility.GetTimeSlots(sitting.StartTime, sitting.EndTime, _timeSlotLength);
            return View(model);
        }
        
        Reservation? reservation = await _context.Reservations
            .Include(r => r.Sitting)
            .Include(r => r.ReservationOrigin)
            .FirstOrDefaultAsync(r => r.Id == model.ReservationId);
        
        if(reservation == null)
            return NotFound();

        reservation.StartTime = model.StartTime;
        reservation.Duration = model.Duration;
        reservation.ReservationOriginId = model.Origin;
        reservation.NumberOfPeople = model.NumGuests;
        reservation.Notes = model.Notes;

        Customer customer =
            await _customerManager.GetOrCreateCustomerAsync(model.FirstName, model.LastName, model.Email, model.Phone);
        reservation.CustomerId = customer.Id;

        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Sitting), new {id = reservation.SittingId, modal = ConfirmationUrl(reservation.Id, true)});
    }
    
    public IActionResult Confirmation(int id, bool? edit)
    {
        Reservation? reservation = _context.Reservations
            .Include(r => r.Sitting)
            .Include(r => r.ReservationOrigin)
            .Include(r => r.Sitting).ThenInclude(s => s.SittingType)
            .FirstOrDefault(r => r.Id == id);

        if (reservation == null)
            return NotFound();

        ViewBag.IsEdit = edit is true;
        
        return View(reservation);
    }

    public async Task<IActionResult> UpdateStatus(int id)
    {
        Reservation? reservation = await _context.Reservations
            .Include(r => r.Customer)
            .FirstOrDefaultAsync(r => r.Id == id);
        
        if(reservation == null)
            return NotFound();

        UpdateStatusViewModel model = new()
        {
            ReservationId = id,
            StatusId = reservation.ReservationStatusId,
            ReservationDetails =
                $"{reservation.Customer.FirstName} {reservation.Customer.LastName} at {reservation.StartTime.ToShortTimeString()}",
            Statuses = new SelectList(await _context.ReservationStatuses.ToListAsync(),
                nameof(ReservationStatus.Id), nameof(ReservationStatus.Description))
        };
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(UpdateStatusViewModel model)
    {
        if (!ModelState.IsValid)
            return Content(string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)), "text/html");

        Reservation? reservation = await _context.Reservations.SingleOrDefaultAsync(r => r.Id == model.ReservationId);
        if(reservation == null)
            return NotFound();
        
        reservation.ReservationStatusId = model.StatusId;
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Sitting), new {id = reservation.SittingId});
    }

    public async Task<IActionResult> AssignTables(int id)
    {
        Reservation? reservation = await _context.Reservations
            .Include(r => r.Sitting).ThenInclude(s => s.Restaurant)
            .ThenInclude(r => r.Areas).ThenInclude(a => a.Tables)
            .Include(r => r.Tables)
            .FirstOrDefaultAsync(r => r.Id == id);
        
        if(reservation == null)
            return NotFound();
        
        AssignTablesViewModel model = new()
        {
            ReservationId = id,
            SittingId = reservation.SittingId,
            Tables = reservation.Sitting.Restaurant.Areas.SelectMany(a => a.Tables).OrderBy(t => t.Id)
                .Select(t => new AssignTablesViewModel.Table {
                Id = t.Id,
                Name = t.Name,
                IsAssigned = reservation.Tables.Any(rt => rt.Id == t.Id)
            }).ToList()
        };
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignTables(AssignTablesViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        Reservation? reservation = await _context.Reservations
            .Include(r => r.Tables)
            .FirstOrDefaultAsync(r => r.Id == model.ReservationId);

        if (reservation == null)
            return NotFound();
        
        reservation.Tables.Clear();
        reservation.Tables.AddRange(model.Tables
            .Where(t => t.IsAssigned)
            .Select(t => _context.Tables.First(tbl => tbl.Id == t.Id)));
        
        await _context.SaveChangesAsync();
        return RedirectToAction("Sitting", new {id=reservation.SittingId});
    }

    private async Task<SelectList> GetAvailableOriginsAsync()
    {
        return new SelectList(await _context.ReservationOrigins.ToListAsync(), 
            nameof(ReservationOrigin.Id), 
            nameof(ReservationOrigin.Description));
    }

    private string ConfirmationUrl(int id, bool edit = false)
    {
        return Url.Action(nameof(Confirmation), new {id, edit}) ?? "";
    }
}