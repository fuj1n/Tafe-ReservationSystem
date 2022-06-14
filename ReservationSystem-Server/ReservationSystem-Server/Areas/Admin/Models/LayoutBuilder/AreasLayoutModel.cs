namespace ReservationSystem_Server.Areas.Admin.Models.LayoutBuilder;

public class AreasLayoutModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public LayoutModel.Rect Rect { get; set; }
}