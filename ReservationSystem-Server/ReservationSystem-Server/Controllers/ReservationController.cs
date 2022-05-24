using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Controllers
{
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
        public IActionResult Index()
        {
            return RedirectToAction("Sittings");
        }

        public async Task<IActionResult> Sittings()
        {
            var sittings = await _context.Sittings.Include(s=>s.SittingType).Where(s=>s.IsClosed == false && s.StartTime>DateTime.Now).ToListAsync();

            return View(sittings);
        }

        public async Task<IActionResult> Create(int sittingId)
        {
            var sitting = await _context.Sittings.
                Include(s=>s.SittingType).FirstOrDefaultAsync(s=>s.Id == sittingId); //retrieves the first sitting that matches the sittinId
            var customer = await _customerManager.FindCustomerAsync(User);

            var vm = new Models.Reservation.CreateVM 
            {
                SittingId = sitting.Id,
                SittingStartTime = sitting.StartTime,
                StartTime = sitting.StartTime,
                SittingEndTime = sitting.EndTime,
                SittingType = sitting.SittingType.Description,
                Duration = TimeSpan.FromMinutes (30), 
                TimeSlots = _utility.GetTimeSlots(sitting.StartTime, sitting.EndTime, TimeSpan.FromMinutes(30))
            };

            if (customer != null)
            {
                vm.FirstName = customer.FirstName;
                vm.LastName = customer.LastName;
                vm.PhoneNumber = customer.PhoneNumber;
                vm.Email = customer.Email;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.Reservation.CreateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.TimeSlots = _utility.GetTimeSlots(model.SittingStartTime, model.SittingEndTime, TimeSpan.FromMinutes(30));
                return View(model);
            }

            var customer = await _customerManager.GetOrCreateCustomerAsync(
                model.FirstName,
                model.LastName,
                model.Email,
                model.PhoneNumber);

            var reservation = new Reservation
            {
                StartTime = model.StartTime,
                Duration = TimeSpan.FromMinutes(30),
                Notes = model.Notes,
                NumberOfPeople = model.NoOfPeople,
                SittingId = model.SittingId,
                CustomerId = customer.Id,
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
                .FirstOrDefaultAsync(r => r.Id == id); //Reservation SittingType
            return View(reservation);
        }
    }
}
