namespace ReservationSystem_Server.Areas.Api.Models.Reservation
{
    public class SittingModel
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool IsClosed { get; set; }

        public int Capacity { get; set; }

        public string SittingType { get; set; } = null!;
    }
}
