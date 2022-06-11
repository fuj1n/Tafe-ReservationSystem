namespace ReservationSystem_Server.Areas.Admin.Models.LayoutBuilder;

public class AreaLayoutModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public LayoutModel.Rect AreaRect { get; set; }
    
    public TableModel[] Tables { get; set; } = null!;
    
    public class TableModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int TableTypeId { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public int Rotation { get; set; }
    }
}