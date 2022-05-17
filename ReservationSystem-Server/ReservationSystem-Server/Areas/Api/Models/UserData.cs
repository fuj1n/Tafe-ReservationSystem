namespace ReservationSystem_Server.Areas.Api.Models;

public class UserData
{
    public bool Authorized { get; set; }
    public string? Username { get; set; }
    public string[] Roles { get; set; } = Array.Empty<string>();
}