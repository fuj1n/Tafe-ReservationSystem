using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem_Server.Areas.Admin.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}