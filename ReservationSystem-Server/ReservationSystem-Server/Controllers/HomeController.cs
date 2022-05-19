using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Areas.Admin.Models.Home;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Models;

namespace ReservationSystem_Server.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.RestaurantCarouselItemVisuals.Where(v => v.RestaurantId == 1).ToArrayAsync());
    }

    public IActionResult Contact(bool submitted = false)
    {
        return View(new ContactViewModel { Submitted = submitted });
    }

    [HttpPost]
    public IActionResult Contact(ContactViewModel vm)
    {
        // No-op for now (this is probably how most of them work anyway)
        if (ModelState.IsValid)
            return RedirectToAction("Contact", new { submitted = true });

        return View(vm);
    }

    public IActionResult TestPalette()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}