namespace ReservationSystem_Server.Areas.Admin.Models.Reservation;

public class SelectTimeViewModel
{
    public int SittingId { get; set; }
    public DateTime SittingStart { get; set; }
    public DateTime SittingEnd { get; set; }

    public List<TimeSlot> TimeSlots { get; set; } = null!;
}