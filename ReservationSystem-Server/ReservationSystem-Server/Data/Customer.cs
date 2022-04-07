namespace ReservationSystem_Server.Data;

public class Customer : Person
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    
    public List<Reservation> Reservations { get; set; } = new();
}