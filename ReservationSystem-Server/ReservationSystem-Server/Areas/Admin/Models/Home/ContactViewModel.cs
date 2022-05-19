namespace ReservationSystem_Server.Areas.Admin.Models.Home;

public class ContactViewModel
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Message { get; set; } = null!;
    public bool Submitted { get; set; }
}