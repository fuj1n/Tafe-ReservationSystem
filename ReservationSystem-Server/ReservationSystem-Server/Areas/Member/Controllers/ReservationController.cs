using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Areas.Member.Controllers;

[Area("Member")]
[Authorize(Roles = "Member")]
public class ReservationController : Controller
{
    private readonly ReservationUtility _reservationUtility;
    private readonly UserManager<IdentityUser> _userManager;

    public ReservationController(ReservationUtility reservationUtility, UserManager<IdentityUser> userManager)
    {
        _reservationUtility = reservationUtility;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        string userId = _userManager.GetUserId(User);
        Reservation[] reservations = await _reservationUtility.GetReservationsForUserAsync(userId, q => q
            .Include(r => r.ReservationOrigin)
            .Include(r => r.ReservationStatus)
            .Include(r => r.Customer)
        );
        
        return View(reservations);
    }

    public async Task<IActionResult> Details(int id)
    {
        string userId = _userManager.GetUserId(User);
        Reservation? reservation = await _reservationUtility.GetReservationAsync(id, q => q
            .Include(r => r.ReservationOrigin)
            .Include(r => r.ReservationStatus)
            .Include(r => r.Customer)
            .Include(r => r.Tables)
            .Include(r => r.Sitting).ThenInclude(s => s.SittingType)
        );
        
        if(reservation == null || reservation.Customer.UserId != userId)
        {
            return NotFound();
        }

        return View(reservation);
    }
}