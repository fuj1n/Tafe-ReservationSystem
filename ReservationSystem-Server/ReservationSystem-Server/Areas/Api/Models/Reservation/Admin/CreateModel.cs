using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReservationSystem_Server.Models;

namespace ReservationSystem_Server.Areas.Api.Models.Reservation.Admin;

public class CreateModel : IValidatable
{
    public int SittingId { get; set; }
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public int ReservationOriginId { get; set; }
    public int NumberOfGuests { get; set; }
    public CustomerModel Customer { get; set; } = new();
    public string? Notes { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        // TODO: move into customer model
        if (string.IsNullOrWhiteSpace(Customer.Email) && string.IsNullOrWhiteSpace(Customer.PhoneNumber))
        {
            modelState.AddModelError(nameof(Customer), "Customer must have an email or phone number");
        }
    }
}