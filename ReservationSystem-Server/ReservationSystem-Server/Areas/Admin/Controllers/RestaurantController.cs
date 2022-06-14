using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem_Server.Areas.Admin.Models.Restaurant;
using ReservationSystem_Server.Data;

namespace ReservationSystem_Server.Areas.Admin.Controllers;

[Authorize(Roles = "Manager")]
[Area("Admin")]
public class RestaurantController : Controller
{
    private readonly ApplicationDbContext _context;
    private const int RestaurantId = 1; // Allow for multi-tenancy later
    
    public RestaurantController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index(int id = RestaurantId)
    {
        Restaurant? restaurant = await _context.Restaurants.FindAsync(id);
        
        if (restaurant == null)
        {
            return NotFound();
        }
        
        return View(restaurant);
    }

    public async Task<IActionResult> Edit(int id = RestaurantId)
    {
        Restaurant? restaurant = await _context.Restaurants.FindAsync(id);
        
        if (restaurant == null)
        {
            return NotFound();
        }

        return View(new EditViewModel
        {
            Id = id,
            Name = restaurant.Name,
            Address = restaurant.Address,
            PhoneNumber = restaurant.PhoneNumber,
            Email = restaurant.Email
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditViewModel vm)
    {
        if (ModelState.IsValid)
        {
            Restaurant? restaurant = await _context.Restaurants.FindAsync(vm.Id);

            if (restaurant == null)
            {
                return NotFound();
            }

            restaurant.Name = vm.Name;
            restaurant.Address = vm.Address;
            restaurant.PhoneNumber = vm.PhoneNumber;
            restaurant.Email = vm.Email;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        // If we got here, something went wrong
        return View(vm);
    }
}