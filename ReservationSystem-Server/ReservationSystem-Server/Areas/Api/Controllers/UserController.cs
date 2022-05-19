using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem_Server.Areas.Api.Models;

namespace ReservationSystem_Server.Areas.Api.Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("me")]
    public async Task<UserData> GetCurrentUser()
    {
        IdentityUser? user = await _userManager.GetUserAsync(User);

        // Not signed in
        if (user == null)
            return new UserData();

        return new UserData
        {
            Authorized = true,
            Username = user.UserName,
            Roles = (await _userManager.GetRolesAsync(user)).ToArray()
        };
    }
}