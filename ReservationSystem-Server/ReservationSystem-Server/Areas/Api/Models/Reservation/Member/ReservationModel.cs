using JetBrains.Annotations;

namespace ReservationSystem_Server.Areas.Api.Models.Reservation.Member;

[PublicAPI]
public class ReservationModel
{
    public int SittingId { get; set; }
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public int ReservationOriginId { get; set; }
    public int ReservationStatusId { get; set; }
    public int NumberOfGuests { get; set; }
    public CustomerModel Customer { get; set; } = new();
    public string? Notes { get; set; }
}