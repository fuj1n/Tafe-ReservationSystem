using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem_Server.Data.Visual;

public class ReservationStatusVisual
{
    [Key, ForeignKey("ReservationStatus")]
    public int Id { get; set; }
    public ReservationStatus ReservationStatus { get; set; } = null!;
    
    public string HtmlBadgeClass { get; set; } = "";
}