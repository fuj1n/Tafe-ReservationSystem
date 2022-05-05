using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Admin.Models.Sitting;
using ReservationSystem_Server.Data;

namespace ReservationSystem_Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager")]
    public class SittingController : Controller
    {
        private ApplicationDbContext _context;
        public SittingController(ApplicationDbContext context)
        {
            _context = context;
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
            // taken from https://stackoverflow.com/questions/1004698/how-to-truncate-milliseconds-off-of-a-net-datetime
            DateTime dt = DateTime.Now;
            dt = new DateTime(dt.Ticks -
                (dt.Ticks % TimeSpan.TicksPerMinute),
                dt.Kind);

            var vm = new CreateVM()
            {
                StartTime = dt,
                EndTime = dt,
                SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description")
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVM vm)
        {
            if(vm.StartTime < DateTime.Now)
            {
                ModelState.AddModelError("StartTime", "Start Time must be in the future");
            }

            vm.Validate(ModelState);

            var sitting = new Sitting
            {
                StartTime = vm.StartTime,
                EndTime = vm.EndTime,
                Capacity = vm.Capacity,
                SittingTypeId = vm.SittingType,
                RestaurantId = 1
            };

            await _context.Sittings.AddAsync(sitting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sitting = await _context.Sittings.FirstOrDefaultAsync(s => s.Id == id);
            var vm = new EditVM()
            {
                Id = id,
                StartTime = sitting!.StartTime,
                EndTime = sitting.EndTime,
                Capacity = sitting.Capacity,
                SittingType = sitting.SittingTypeId,
                SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description")
            };
            return View(vm);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(EditVM vm)
        {
            var sitting = await _context.Sittings.FirstOrDefaultAsync(s => s.Id == vm.Id);
            if (sitting == null)
            {
                return NotFound();
            }

            vm.Validate(ModelState);

            if (!ModelState.IsValid)
            {
                vm.SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description");
                return View(vm);
            }
            sitting.StartTime = vm.StartTime;
            sitting.EndTime = vm.EndTime;
            sitting.Capacity = vm.Capacity;
            sitting.SittingTypeId = vm.SittingType;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpPut]
        public async Task<IActionResult> Close(int id)
        {
            var sitting = await _context.Sittings.FirstOrDefaultAsync(s => s.Id == id);
            sitting!.IsClosed = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
