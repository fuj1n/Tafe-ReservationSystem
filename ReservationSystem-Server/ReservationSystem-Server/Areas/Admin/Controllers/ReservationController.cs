using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Admin.Models.Reservation;
using ReservationSystem_Server.Data;

namespace ReservationSystem_Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Employee")]
public class ReservationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly TimeSpan _timeSlotLength = TimeSpan.FromMinutes(30);

    public ReservationController(ApplicationDbContext context)
    {
        _context = context;
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
            .FirstOrDefaultAsync(s => s.Id == id);
        if (sitting == null)
        {
            return NotFound();
        }
        
        return View(new SittingViewModel
        {
            Sitting = sitting,
            ReservationStatusVisuals = await _context.ReservationStatusVisuals
                .ToDictionaryAsync(v => v.Id, v => v)
        });
    }

    public async Task<IActionResult> SelectTime(int sittingId)
    {
        Sitting? sitting = await _context.Sittings
            .FirstOrDefaultAsync(s => s.Id == sittingId);
        if (sitting == null)
        {
            return NotFound();
        }
        
        SelectTimeViewModel model = new()
        {
            SittingId = sittingId,
            SittingStart = sitting.StartTime,
            SittingEnd = sitting.EndTime,
            TimeSlots = GetTimeSlots(sitting)
        };

        return View(model);
    }

    public async Task<IActionResult> Create(int sittingId, long start)
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
            
            StartTime = new DateTime(start),
            
            AvailableOrigins = new SelectList(await _context.ReservationOrigins.ToListAsync(), 
                nameof(ReservationOrigin.Id), nameof(ReservationOrigin.Description))
        };
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        Sitting? sitting = await _context.Sittings
            .FirstOrDefaultAsync(s => s.Id == model.SittingId);

        if (sitting == null)
        {
            return NotFound();
        }
        
        Reservation reservation = new()
        {
            SittingId = model.SittingId,
            StartTime = model.StartTime,
            Duration = model.Duration,
            NumberOfPeople = model.NumGuests,
            ReservationOriginId = model.Origin,
            Notes = model.Notes,
            ReservationStatusId = 1,
            CustomerId = 1
        };
        
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Confirmation), new {id = reservation.Id});
    }
    
    public IActionResult Confirmation(int id)
    {
        Reservation? reservation = _context.Reservations
            .Include(r => r.Sitting)
            .Include(r => r.ReservationOrigin)
            .Include(r => r.Sitting).ThenInclude(s => s.SittingType)
            .FirstOrDefault(r => r.Id == id);

        if (reservation == null)
            return NotFound();
        
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
    
    private List<DateTime> GetTimeSlots(Sitting sitting)
    {
        List<DateTime> timeSlots = new();
        
        TimeSpan sittingDuration = sitting.EndTime - sitting.StartTime;
        
        //TODO: configurable time slot length
        for (TimeSpan time = new(0); time < sittingDuration; time += _timeSlotLength)
        {
            timeSlots.Add(sitting.StartTime + time);
        }

        return timeSlots;
    }
}