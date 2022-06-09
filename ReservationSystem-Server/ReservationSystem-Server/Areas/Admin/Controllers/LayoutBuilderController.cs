using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem_Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Manager")]
public class LayoutBuilderController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}