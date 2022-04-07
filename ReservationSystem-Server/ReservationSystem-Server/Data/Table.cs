namespace ReservationSystem_Server.Data;

public class Table
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public int Seats { get; set; }
    
    public int AreaId { get; set; }
    public RestaurantArea Area { get; set; } = null!;
    
    public List<Reservation> Reservations { get; set; } = new();
}