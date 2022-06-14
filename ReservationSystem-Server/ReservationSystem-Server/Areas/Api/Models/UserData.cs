using JetBrains.Annotations;
using ReservationSystem_Server.Data;

namespace ReservationSystem_Server.Areas.Api.Models;

[PublicAPI]
public class UserData
{
    public bool Authorized { get; set; }
    public string? Username { get; set; }
    public string[] Roles { get; set; } = Array.Empty<string>();
    public Person? Person { get; set; }
}