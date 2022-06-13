using System.ComponentModel.DataAnnotations;

namespace ReservationSystem_Server.Areas.Admin.Models.Employee;

public class EditViewModel
{
    public string Id { get; set; } = null!;
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Phone]
    public string? PhoneNumber { get; set; }
}