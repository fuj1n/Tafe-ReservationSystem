using System.ComponentModel.DataAnnotations;

namespace ReservationSystem_Server.Areas.Admin.Models.Employee;

public class CreateViewModel
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Phone]
    public string? PhoneNumber { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;
}