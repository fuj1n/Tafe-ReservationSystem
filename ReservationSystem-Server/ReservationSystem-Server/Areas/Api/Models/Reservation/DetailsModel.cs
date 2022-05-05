namespace ReservationSystem_Server.Areas.Api.Models.Reservation
{
    public class DetailsModel
    {
        public int SittingId { get; set; }

        public TimeSpan Duration { get; set; }

        public List<DateTime>? TimeSlots { get; set; }
    }
}
