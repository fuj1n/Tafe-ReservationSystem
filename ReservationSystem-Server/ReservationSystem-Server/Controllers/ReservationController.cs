using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;

namespace ReservationSystem_Server.Controllers
{
    public class ReservationController : Controller
    {
        private ApplicationDbContext _context;
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
            var sittings = await _context.Sittings.ToListAsync();

            return View(sittings);
        }

        public async Task<IActionResult> Create(int sittingId)
        {
            var sitting = await _context.Sittings.Include(s=>s.SittingType).FirstAsync(s=>s.Id == sittingId);

            var vm = new Models.Reservation.CreateVM 
            {
                SittingId = sitting.Id,
                SittingStartTime = sitting.StartTime,
                SittingEndTime = sitting.EndTime,
                SittingType = sitting.SittingType.Description

            };

            return View(vm);
        }
    }
}
