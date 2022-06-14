namespace ReservationSystem_Server.Data;

public class Sitting
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public bool IsClosed { get; set; }
    
    public int Capacity { get; set; }
    public TimeSpan DefaultDuration { get; set; } = TimeSpan.FromMinutes(30);
    
    public int SittingTypeId { get; set; }
    public SittingType SittingType { get; set; } = null!;
    
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    
    public List<Reservation> Reservations { get; set; } = new();
}