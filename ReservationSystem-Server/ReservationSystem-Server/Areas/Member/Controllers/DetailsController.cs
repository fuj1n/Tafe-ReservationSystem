using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem_Server.Areas.Member.Controllers
{
    public class DetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
