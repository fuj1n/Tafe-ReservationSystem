using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Admin.Models.Sitting;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Helper;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager")]
    public class SittingController : Controller
    {
        private readonly SittingUtility _sittingUtility;
        private ApplicationDbContext _context;
        public SittingController(ApplicationDbContext context, SittingUtility sittingUtility)
        {
            _context = context;
            _sittingUtility = sittingUtility;
        }

        // taken from https://stackoverflow.com/questions/1004698/how-to-truncate-milliseconds-off-of-a-net-datetime
        // modified to snap to nearest 5 minutes
        private DateTime DateTimeTruncate(DateTime dateTime)
        {
            return new DateTime(dateTime.Ticks - (dateTime.Ticks % (TimeSpan.TicksPerMinute * 5)), dateTime.Kind);
        }

        public async Task<IActionResult> Index(bool pastSittings)
        {
            ViewBag.pastSittings = pastSittings;
            var sittings = await _context.Sittings.Include(s => s.SittingType).OrderBy(s => s.StartTime)
                .Where(s => pastSittings || s.StartTime > DateTime.Now).ToListAsync();
            return View(sittings);
        }

        public async Task<IActionResult> Create()
        {

            DateTime dt = DateTimeTruncate(DateTime.Now);

            var vm = new CreateVM()
            {
                StartTime = dt,
                EndTime = dt,
                DefaultDuration = TimeSpan.FromMinutes(30),
                SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description")
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVM vm)
        {
            vm.Validate(ModelState);

            var sitting = new Sitting
            {
                StartTime = vm.StartTime,
                EndTime = vm.EndTime,
                DefaultDuration = vm.DefaultDuration,
                Capacity = vm.Capacity,
                SittingTypeId = vm.SittingTypeId,
                RestaurantId = 1
            };

            if (!ModelState.IsValid)
            {
                vm.SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description");
                return View(vm);
            }

            await _context.Sittings.AddAsync(sitting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Confirmation), new { id = sitting.Id });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sitting = await _context.Sittings.Include(s => s.Reservations).FirstOrDefaultAsync(s => s.Id == id);
            if (sitting == null)
            {
                return NotFound();
            }

            bool reservations = false;
            if( sitting.Reservations.Count != 0 )
            {
                reservations = true;
            }
            ViewBag.reservations = reservations;

            var vm = new EditVM()
            {
                Id = id,
                StartTime = DateTimeTruncate(sitting.StartTime),
                EndTime = DateTimeTruncate(sitting.EndTime),
                DefaultDuration = sitting.DefaultDuration,
                Capacity = sitting.Capacity,
                SittingTypeId = sitting.SittingTypeId,
                SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description")
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditVM vm)
        {
            var sitting = await _context.Sittings.Include(s => s.Reservations).FirstOrDefaultAsync(s => s.Id == vm.Id);
            if (sitting == null)
            {
                return NotFound();
            }

            vm.Validate(ModelState);

            if (!ModelState.IsValid)
            {
                vm.SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description");

                bool reservations = false;
                if (sitting.Reservations.Count != 0)
                {
                    reservations = true;
                }
                ViewBag.reservations = reservations;

                return View(vm);
            }
            sitting.StartTime = vm.StartTime;
            sitting.EndTime = vm.EndTime;
            sitting.DefaultDuration = vm.DefaultDuration;
            sitting.Capacity = vm.Capacity;
            sitting.SittingTypeId = vm.SittingTypeId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Confirmation), new { id = sitting.Id, edit = true });

        }

        public async Task<IActionResult> Close(int id)
        {
            return View(id);
        }

        [HttpPost, ActionName("Close")]
        public async Task<IActionResult> DoClose(int id)
        {
            var sitting = await _context.Sittings.FirstOrDefaultAsync(s => s.Id == id);
            if (sitting == null)
            {
                return NotFound();
            }
            sitting.IsClosed = true;
            await _context.SaveChangesAsync();
            return this.CloseModalAndRefresh();
        }

        public async Task<IActionResult> Confirmation(int id, bool? edit)
        {
            Sitting? sitting = await _sittingUtility.GetSittingAsync(id, q => q
                .Include(r => r.SittingType));

            if (sitting == null)
                return NotFound();

            ViewBag.IsEdit = edit is true;

            return View(sitting);
        }
    }
}
