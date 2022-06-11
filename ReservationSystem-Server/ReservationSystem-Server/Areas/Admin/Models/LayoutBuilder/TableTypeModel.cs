namespace ReservationSystem_Server.Areas.Admin.Models.LayoutBuilder;

public class TableTypeModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Seats { get; set; }
    
    public float Width { get; set; }
    public float Height { get; set; }

    public LayoutModel.Rect[] Rects { get; set; } = null!;
}