using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Models.Reservation.Api;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Controllers.Api
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ReservationUtility _utility;
        private readonly CustomerManager _customerManager;
        public ReservationController(ApplicationDbContext context, ReservationUtility utility, CustomerManager customerManager)
        {
            _context = context;
            _utility = utility;
            _customerManager = customerManager;
        }

        [HttpGet("sittings")]
        public async Task<IActionResult> Sittings()
        {
            var sittings = await _context.Sittings.Include(s => s.SittingType).Where(s => s.IsClosed == false && s.StartTime > DateTime.Now)
                            .Select(s=>new SittingVM {
                                Id = s.Id,
                                StartTime = s.StartTime,
                                EndTime = s.EndTime,
                                IsClosed = s.IsClosed,
                                Capacity = s.Capacity,
                                SittingType = s.SittingType.Description
                            }).ToListAsync();


            return Ok(sittings);
        }
    }
}
