using Microsoft.AspNetCore.Identity;

namespace ReservationSystem_Server.Data;

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    
    public string? UserId { get; set; }
    public IdentityUser? User { get; set; }
}