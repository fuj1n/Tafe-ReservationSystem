using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem_Server.Data.Visual.Layout;

public class TableVisual
{
    [Key, ForeignKey(nameof(Table))]
    public int TableId { get; set; }
    public Table Table { get; set; } = null!;
    
    public int TableTypeId { get; set; }
    public TableTypeVisual TableType { get; set; } = null!;
    
    public float X { get; set; }
    public float Y { get; set; }
    public int Rotation { get; set; }
}