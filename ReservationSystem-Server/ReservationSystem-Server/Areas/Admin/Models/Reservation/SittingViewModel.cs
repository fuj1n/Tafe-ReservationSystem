using ReservationSystem_Server.Data;
using ReservationSystem_Server.Data.Visual;

namespace ReservationSystem_Server.Areas.Admin.Models.Reservation;

public class SittingViewModel
{
    public Sitting Sitting { get; set; } = null!;
    public Dictionary<int, ReservationStatusVisual> ReservationStatusVisuals { get; set; } = null!;
}