using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationSystem_Server.Models;

namespace ReservationSystem_Server.Areas.Admin.Models.Reservation;

public class CreateViewModel
{
    public int SittingId { get; set; }
    public DateTime SittingStart { get; set; }
    public DateTime SittingEnd { get; set; }
    
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(30);

    public int Origin { get; set; }
    
    [Range(0, 1000)]
    public int NumGuests { get; set; }
    
    public string? Notes { get; set; }
    
    public SelectList? AvailableOrigins { get; set; }
    public List<DateTime>? TimeSlots { get; set; }
}