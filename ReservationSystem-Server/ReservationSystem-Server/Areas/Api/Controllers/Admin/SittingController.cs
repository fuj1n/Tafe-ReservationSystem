using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Admin.Models.Sitting;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Areas.Api.Controllers.Admin
{
    [ApiController]
    [Route("/api/v1/admin/[controller]")]
    [Area("Api")]
    [Authorize(Roles = "Manager")]
    public class SittingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SittingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("sittings")]
        public async Task<IActionResult> Index(bool pastSittings)
        {
            ViewBag.pastSittings = pastSittings;
            var sittings = await _context.Sittings.Include(s => s.SittingType).OrderBy(s => s.StartTime)
                .Where(s => pastSittings || s.StartTime > DateTime.Now).ToListAsync();
            return Ok(sittings);
        }

        [HttpGet("close")]
        public async Task<IActionResult> Close(int id)
        {
            var sitting = await _context.Sittings.FirstOrDefaultAsync(s => s.Id == id);
            if (sitting == null)
            {
                return NotFound();
            }

            sitting.IsClosed = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("sittingTypes")]
        public async Task<IActionResult> SittingTypes()
        {
            var sittingTypes = await _context.SittingTypes.ToListAsync();
            return Ok(sittingTypes);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateVM vm)
        {
            vm.Validate(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
            }
            var sitting = new Sitting
            {
                StartTime = vm.StartTime,
                EndTime = vm.EndTime,
                Capacity = vm.Capacity,
                SittingTypeId = vm.SittingTypeId,
                RestaurantId = 1
            };

            _context.Sittings.Add(sitting);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Create", sitting);
        }

        [HttpPost("edit")]
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
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
            }
            sitting.StartTime = vm.StartTime;
            sitting.EndTime = vm.EndTime;
            sitting.Capacity = vm.Capacity;
            sitting.SittingTypeId = vm.SittingTypeId;

            await _context.SaveChangesAsync();
            return Ok(sitting);

        }
    }
}
