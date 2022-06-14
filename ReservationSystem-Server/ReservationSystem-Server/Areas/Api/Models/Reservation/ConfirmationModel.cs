namespace ReservationSystem_Server.Areas.Api.Models.Reservation
{
    public class ConfirmationModel
    {
        public string SittingType { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public int NoOfPeople { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public string? Notes { get; set; }
    }
}
