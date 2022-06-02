using ReservationSystem_Server.Data;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem_Server.Models.Reservation
{
    public class CreateVM
    {
        public int SittingId { get; set; }
        public DateTime SittingStartTime { get; set; }   //Sitting Start time decided by the manager
        public DateTime SittingEndTime { get; set; }   //Sitting End time decided by the manager

        public string SittingType { get; set; }

        public DateTime StartTime { get; set; }  //Sitting End time not needed in VM as user do not input this
        public TimeSpan DefaultDuration { get ; set; }

        [Range(0, 10, ErrorMessage = "Bookings of more than 10 must be made by contacting the restaurant")]
        [Required(ErrorMessage = "Number of guests must be at least 1")]
        public int NoOfPeople { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Notes { get; set; }

        public List<DateTime>? TimeSlots { get; set; }

    }
}
