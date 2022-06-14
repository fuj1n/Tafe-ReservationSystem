using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Admin.Models.Sitting;
using ReservationSystem_Server.Areas.Api.Models.Sitting;
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
        private readonly SittingUtility _sittingUtility;
        public SittingController(ApplicationDbContext context, SittingUtility sittingUtility)
        {
            _context = context;
            _sittingUtility = sittingUtility;
        }

        [HttpGet("sittings")]
        public async Task<IActionResult> Index(bool pastSittings)
        {
            ViewBag.pastSittings = pastSittings;
            var sittings = await _context.Sittings.Include(s => s.SittingType).OrderBy(s => s.StartTime)
                .Where(s => pastSittings || s.StartTime > DateTime.Now).ToListAsync();
            return Ok(sittings);
        }

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

        [HttpPut("close")]
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
                return BadRequest(ModelState);
            }
            var sitting = new Sitting
            {
                StartTime = vm.StartTime,
                EndTime = vm.EndTime,
                DefaultDuration = vm.DefaultDuration,
                Capacity = vm.Capacity,
                SittingTypeId = vm.SittingTypeId,
                RestaurantId = 1
            };

            _context.Sittings.Add(sitting);
            await _context.SaveChangesAsync();

            await _context.SittingTypes.ToArrayAsync();
            return CreatedAtAction("Create", sitting);
        }

        [HttpPut("edit")]
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
                return BadRequest(ModelState);
            }
            sitting.StartTime = vm.StartTime;
            sitting.EndTime = vm.EndTime;
            sitting.DefaultDuration = vm.DefaultDuration;
            sitting.Capacity = vm.Capacity;
            sitting.SittingTypeId = vm.SittingTypeId;

            await _context.SaveChangesAsync();

            await _context.SittingTypes.ToArrayAsync();
            return Ok(sitting);

        }
    }
}
