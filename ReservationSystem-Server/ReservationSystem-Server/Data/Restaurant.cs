namespace ReservationSystem_Server.Data;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;

    public List<RestaurantArea> Areas { get; set; } = new();
    public List<Sitting> Sittings { get; set; } = new();
}