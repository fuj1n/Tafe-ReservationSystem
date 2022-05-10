namespace ReservationSystem_Server.Data;

public class Reservation
{
    public int Id { get; set; }
    
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime EndTime => StartTime + Duration;

    public string? Notes { get; set; }
    
    public int NumberOfPeople { get; set; }
    
    public int SittingId { get; set; }
    public Sitting Sitting { get; set; } = null!;
    
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    
    public int ReservationStatusId { get; set; }
    public ReservationStatus ReservationStatus { get; set; } = null!;
    
    public int ReservationOriginId { get; set; }
    public ReservationOrigin ReservationOrigin { get; set; } = null!;
    
    public List<Table> Tables { get; set; } = new();

    public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();
}