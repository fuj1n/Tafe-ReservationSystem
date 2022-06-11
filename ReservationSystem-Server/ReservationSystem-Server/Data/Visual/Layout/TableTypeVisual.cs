namespace ReservationSystem_Server.Data.Visual.Layout;

public class TableTypeVisual
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Seats { get; set; }
    
    public float Width { get; set; }
    public float Height { get; set; }

    public List<RectangleVisual> Rects { get; set; } = new();
}