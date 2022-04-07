
namespace ReservationSystem_Server.Data;

public class RestaurantArea
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;

    public List<Table> Tables { get; set; } = new();
}