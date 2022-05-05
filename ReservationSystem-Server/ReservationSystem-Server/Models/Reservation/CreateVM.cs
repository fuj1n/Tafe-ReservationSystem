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
        public TimeSpan Duration { get ; set; }

        [Range(0,1000)]
        public int NoOfPeople { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Notes { get; set; }

        public List<DateTime>? TimeSlots { get; set; }

    }
}
