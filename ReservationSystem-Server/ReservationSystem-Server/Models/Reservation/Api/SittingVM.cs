namespace ReservationSystem_Server.Models.Reservation.Api
{
    public class SittingVM
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool IsClosed { get; set; }

        public int Capacity { get; set; }

        public string SittingType { get; set; } = null!;
    }
}
