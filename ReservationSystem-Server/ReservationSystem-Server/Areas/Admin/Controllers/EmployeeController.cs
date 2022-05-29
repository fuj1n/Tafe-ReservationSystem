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
    private readonly RoleManager<IdentityRole> _roleManager;

    public EmployeeController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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
    
    public async Task<IActionResult> Delete(string id)
    {
        if(_userManager.GetUserId(User).Equals(id, StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction(nameof(ErrorMessage), new { message = "Cannot delete self." });
        }

        IdentityUser? user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }
        
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DoDelete(string id)
    {
        if(_userManager.GetUserId(User).Equals(id, StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction(nameof(ErrorMessage), new { message = "Cannot delete self." });
        }

        IdentityUser? user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }
        
        IdentityResult? deleteResult = await _userManager.DeleteAsync(user);

        if (deleteResult.Succeeded)
        {
            return RedirectToAction("Index");
        }
        
        foreach (IdentityError error in deleteResult.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(user);
    }

    public async Task<IActionResult> ResetPassword(string id)
    {
        IdentityUser? user = await _userManager.FindByIdAsync(id);
        
        if (user == null)
        {
            return NotFound();
        }
        
        return View(new ResetPasswordViewModel
        {
            Id = user.Id,
            Email = user.Email
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
    {
        if (ModelState.IsValid)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(vm.Id);
            
            if(user == null)
            {
                return NotFound();
            }
            
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult? resetResult = await _userManager.ResetPasswordAsync(user, token, vm.Password);
            
            if(resetResult.Succeeded)
            {
                return RedirectToAction("Index");
            }
            
            foreach (IdentityError error in resetResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        
        // If we got here, something went wrong
        return View(vm);
    }

    public async Task<IActionResult> Roles(string id)
    {
        IdentityUser? user = await _userManager.FindByIdAsync(id);
        
        if (user == null)
        {
            return NotFound();
        }
        
        HashSet<string> userRoles = new(await _userManager.GetRolesAsync(user));
        RolesViewModel.RoleModel[] roles = _roleManager.Roles.Where(r => r.Name != "Employee").Select(r => new RolesViewModel.RoleModel
        {
            Id = r.Id,
            Name = r.Name,
            IsSelected = userRoles.Contains(r.Name)
        }).ToArray();

        return View(new RolesViewModel
        {
            Id = id,
            Email = user.Email,
            Roles = roles
        });
    }

    [HttpPost]
    public async Task<IActionResult> Roles(RolesViewModel vm)
    {
        IdentityUser? user = await _userManager.FindByIdAsync(vm.Id);
        
        if (user == null)
        {
            return NotFound();
        }
        
        IList<string> currentRoles = await _userManager.GetRolesAsync(user);
        IdentityResult removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles.Where(r => r != "Employee"));

        if (removeResult.Succeeded)
        {
            IdentityResult addResult = await _userManager.AddToRolesAsync(user, vm.Roles.Where(r => r.IsSelected).Select(r => r.Name));
            
            if (addResult.Succeeded)
            {
                return RedirectToAction("Index");
            }
            
            foreach (IdentityError error in addResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        else
        {
            foreach (IdentityError error in removeResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(vm);
    }
    
    public IActionResult ErrorMessage(string message)
    {
        return View((object)message);
    }
}