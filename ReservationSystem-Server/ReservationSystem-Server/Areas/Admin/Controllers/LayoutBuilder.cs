using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem_Server.Areas.Admin.Controllers;

public class LayoutBuilder : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}