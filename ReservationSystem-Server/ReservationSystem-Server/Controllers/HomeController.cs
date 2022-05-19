using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem_Server.Models;

namespace ReservationSystem_Server.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        string File(string name) => Url.Content($"~/images/home-carousel/{name}.webp");

        List<(string, string)> carouselImages = new()
        {
                (File("OutdoorGarden"), "Enjoy the fresh air in our outdoor garden"),
                (File("BlueberryMuffin"), "Freshly baked muffins everyday"),
                (File("Cakes"), "Scrumptous cakes freshly baked")
        };
        
        return View(carouselImages);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}