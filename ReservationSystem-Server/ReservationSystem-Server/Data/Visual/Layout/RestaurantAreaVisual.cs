using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem_Server.Data.Visual.Layout;

public class RestaurantAreaVisual
{
    [Key, ForeignKey(nameof(Area))]
    public int AreaId { get; set; }
    public RestaurantArea Area { get; set; } = null!;
    
    public RectangleVisual Rect { get; set; } = null!;
}