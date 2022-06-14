using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem_Server.Data;

namespace ReservationSystem_Server.Areas.Api.Controllers;

#if DEBUG // Only for development, don't really want random users to be able to make themselves admin otherwise
[ApiController]
[Route("api/v1/hidden_test/[controller]")]
[Area("Api")]
public class TestApiController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;
    
    public TestApiController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    
    [HttpGet("MyClaims")]
    public IEnumerable<string> MyClaims()
    {
        var identity = User.Identity as ClaimsIdentity;
        
        
        
        return identity!.Claims.Select(c => $"{c.Type} - {c.Value}");
    }
    
    [HttpGet("AddRole/{role}")]
    public async Task<IActionResult> AddRole(string role)
    {
        IdentityUser? user = await _userManager.GetUserAsync(User);
        IdentityResult result = await _userManager.AddToRoleAsync(user, role);

        await _signInManager.RefreshSignInAsync(user); // update role immediately without re-sign-in
        
        return result.Succeeded ? Ok() : BadRequest();
    }
    
    [HttpGet("AddCustomer/{name}/{lastName}/{email}/{phone}")]
    public async Task<IActionResult> AddCustomer(string name, string lastName, string email, string phone)
    {
        Customer customer = new()
        {
            FirstName = name,
            LastName = lastName,
            Email = email,
            PhoneNumber = phone
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        
        return Ok();
    }
}
#endif