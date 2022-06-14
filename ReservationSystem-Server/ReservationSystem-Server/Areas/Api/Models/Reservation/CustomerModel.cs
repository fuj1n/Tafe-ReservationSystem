namespace ReservationSystem_Server.Areas.Api.Models.Reservation
{
    public class CustomerModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
