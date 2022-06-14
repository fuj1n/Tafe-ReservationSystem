using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem_Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Employee")]
public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}