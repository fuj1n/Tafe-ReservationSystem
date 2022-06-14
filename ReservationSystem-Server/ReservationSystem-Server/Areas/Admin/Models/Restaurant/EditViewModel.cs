using System.ComponentModel.DataAnnotations;

namespace ReservationSystem_Server.Areas.Admin.Models.Restaurant;

public class EditViewModel
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = null!;
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
}