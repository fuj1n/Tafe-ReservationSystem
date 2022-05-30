using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Api.Models.Reservation;
using ReservationSystem_Server.Areas.Api.Models.Reservation.Member;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Areas.Api.Controllers.Member;

[Area("Api")]
[Authorize(Roles = "Member")]
[Route($"{ApiInfo.Base}member/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly ReservationUtility _reservationUtility;
    private readonly UserManager<IdentityUser> _userManager;

    public ReservationController(ReservationUtility reservationUtility, UserManager<IdentityUser> userManager)
    {
        _reservationUtility = reservationUtility;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        string userId = _userManager.GetUserId(User);
        Reservation[] reservations = await _reservationUtility.GetReservationsForUserAsync(userId, q => q
            .Include(r => r.ReservationOrigin)
            .Include(r => r.ReservationStatus)
            .Include(r => r.Customer)
        );
        
        return Ok(reservations.Select(r => new ReservationModel
        {
            SittingId = r.SittingId,
            ReservationOriginId = r.ReservationOriginId,
            ReservationStatusId = r.ReservationStatusId,
            Customer = new CustomerModel
            {
                FirstName = r.Customer.FirstName,
                LastName = r.Customer.LastName,
                Email = r.Customer.Email,
                PhoneNumber = r.Customer.PhoneNumber
            },
            NumberOfGuests = r.NumberOfPeople,
            Duration = r.Duration,
            StartTime = r.StartTime,
            Notes = r.Notes
        }));
    }
}