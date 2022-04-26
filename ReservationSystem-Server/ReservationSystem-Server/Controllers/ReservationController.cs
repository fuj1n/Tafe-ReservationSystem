using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;

namespace ReservationSystem_Server.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context; 
        public ReservationController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Sittings()
        {
            var sittings = await _context.Sittings.Include(s=>s.SittingType).Where(s=>s.IsClosed == false && s.StartTime>DateTime.Now).ToListAsync();

            return View(sittings);
        }

        public async Task<IActionResult> Create(int sittingId)
        {
            var sitting = await _context.Sittings.
                Include(s=>s.SittingType).FirstOrDefaultAsync(s=>s.Id == sittingId);

            var vm = new Models.Reservation.CreateVM 
            {
                SittingId = sitting.Id,
                SittingStartTime = sitting.StartTime,
                StartTime = sitting.StartTime,
                SittingEndTime = sitting.EndTime,
                SittingType = sitting.SittingType.Description,
                Duration = TimeSpan.FromMinutes (30)

            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.Reservation.CreateVM model)
        {
            var reservation = new Reservation
            {
                StartTime = model.StartTime,
                Duration = model.Duration,
                Notes = model.Notes,
                NumberOfPeople = model.NoOfPeople,
                SittingId = model.SittingId,
                CustomerId = 1,
                ReservationStatusId = 1,
                ReservationOriginId = 4
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Confirmation", new {id=reservation.Id});
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var reservation = await _context.Reservations
                .Include(r=>r.Sitting)
                .ThenInclude(s=>s.SittingType)
                .FirstOrDefaultAsync(r => r.Id == id);
            return View(reservation);
        }
    }
}
