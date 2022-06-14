using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReservationSystem_Server.Areas.Admin.Models.Reservation;

public class UpdateStatusViewModel
{
    public int ReservationId { get; set; }
    public int StatusId { get; set; }

    public string? ReservationDetails { get; set; }
    
    public SelectList? Statuses { get; set; }
}