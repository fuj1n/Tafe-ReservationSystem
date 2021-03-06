using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Api.Models.Reservation;
using ReservationSystem_Server.Areas.Api.Models.Reservation.Admin;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Models.Reservation;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Areas.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [Area("Api")]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ReservationUtility _reservationUtility;
        private readonly SittingUtility _sittingUtility;
        private readonly CustomerManager _customerManager;
        public ReservationController(ApplicationDbContext context, ReservationUtility reservationUtility, SittingUtility sittingUtility, CustomerManager customerManager)
        {
            _context = context;
            _reservationUtility = reservationUtility;
            _sittingUtility = sittingUtility;
            _customerManager = customerManager;
        }

        [HttpGet("sittings")]
        public async Task<IActionResult> Sittings()
        {
            var sittings = await _context.Sittings.Include(s => s.SittingType).Where(s => s.IsClosed == false && s.StartTime > DateTime.Now)
                            .Select(s => new SittingModel
                            {
                                Id = s.Id,
                                StartTime = s.StartTime,
                                EndTime = s.EndTime,
                                IsClosed = s.IsClosed,
                                Capacity = s.Capacity,
                                SittingType = s.SittingType.Description
                            }).OrderBy(s=>s.StartTime).ToListAsync();


            return Ok(sittings);
        }

        [HttpGet("details")]
        public async Task<IActionResult> Details(int sittingId)
        {
            var sitting = await _sittingUtility.GetSittingAsync(sittingId);
            //var sitting = await _context.Sittings.FirstOrDefaultAsync(s => s.Id == sittingId);

            if (sitting == null)
            {
                return NotFound();
            }

            var details = new DetailsModel
            {

                SittingId = sitting.Id,
                Duration = sitting.DefaultDuration,
                TimeSlots = _reservationUtility.GetTimeSlots(sitting.StartTime, sitting.EndTime, sitting.DefaultDuration)
            };

            return Ok(details);
        }

        [HttpGet("userinfo")]
        public async Task<IActionResult> UserInfo()
        {
            var user = await _customerManager.FindCustomerAsync(User); //finds the customer according to the currently logged in user

            if (user == null)
            {
                return NoContent();
            }

            var info = new CustomerModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return Ok(info);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ConfirmationModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateVM model)
        {
            //if (model.NoOfPeople > 10)
            //{
            //    ModelState.AddModelError("NoOfPeople", "Bookings of more than 10 must be made with a restaurant staff, please contact restaurant using details below");
            //}

            if (!ModelState.IsValid)  //ModelState contains all the errors
            {
                return BadRequest(ModelState); //For every value in ModelState, selects all lists of errors and returns them as 1 long list removed .Values.SelectMany(v => v.Errors
            }

            var sitting = await _sittingUtility.GetSittingAsync(model.SittingId, s =>
                s.Include(s => s.SittingType));

            //var sitting = await _context.Sittings
            //         .Include(s => s.SittingType).FirstOrDefaultAsync(s => s.Id == model.SittingId);
            if (sitting == null)
            {
                return NotFound();
            }

            var customer = await _customerManager.GetOrCreateCustomerAsync(
              model.FirstName,
              model.LastName,
              model.Email,
              model.PhoneNumber);

            var reservation = new Reservation
            {
                StartTime = model.StartTime,
                Duration = sitting.DefaultDuration,
                Notes = model.Notes,
                NumberOfPeople = model.NoOfPeople,
                SittingId = model.SittingId,
                CustomerId = customer.Id,
                ReservationStatusId = 1,
                ReservationOriginId = 4
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var confirmation = new ConfirmationModel
            {
                
                SittingType = sitting.SittingType.Description,
                StartTime = model.StartTime,
                Duration = sitting.DefaultDuration,
                NoOfPeople = model.NoOfPeople,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.PhoneNumber,
                Email = model.Email,
                Notes = model.Notes,
            };

            return CreatedAtAction(nameof(Create), confirmation); //nameof converts the Create method name into a string
        }

    }


}

