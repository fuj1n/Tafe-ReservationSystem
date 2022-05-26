using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem_Server.Areas.Admin.Models.Employee;

namespace ReservationSystem_Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class EmployeeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    
    public EmployeeController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _userManager.GetUsersInRoleAsync("Employee"));
    }

    public IActionResult Create()
    {
        return View(new CreateViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateViewModel vm)
    {
        if (ModelState.IsValid)
        {
            IdentityUser user = new()
            {
                UserName = vm.Email,
                Email = vm.Email,
                EmailConfirmed = true,
                PhoneNumber = vm.PhoneNumber,
                PhoneNumberConfirmed = true
            };

            IdentityResult? createResult = await _userManager.CreateAsync(user, vm.Password);

            if (createResult.Succeeded)
            {
                IdentityResult? addRoleResult = await _userManager.AddToRoleAsync(user, "Employee");
                
                if(addRoleResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                
                foreach (IdentityError error in addRoleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                foreach (IdentityError error in createResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }

        // If we got here, something went wrong
        return View(vm);
    }

    public async Task<IActionResult> Edit(string id)
    {
        IdentityUser? user = await _userManager.FindByIdAsync(id);
        
        if(user == null)
        {
            return NotFound();
        }
        
        return View(new EditViewModel
        {
            Id = user.Id,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditViewModel vm)
    {
        if (ModelState.IsValid)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(vm.Id);
            
            if(user == null)
            {
                return NotFound();
            }
            
            user.UserName = vm.Email;
            user.Email = vm.Email;
            user.PhoneNumber = vm.PhoneNumber;
            
            IdentityResult? updateResult = await _userManager.UpdateAsync(user);
            
            if(updateResult.Succeeded)
            {
                IdentityResult? usernameResult = await _userManager.SetUserNameAsync(user, vm.Email);
                
                if(usernameResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                
                foreach (IdentityError error in usernameResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                foreach (IdentityError error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }
        
        // If we got here, something went wrong
        return View(vm);
    }
}