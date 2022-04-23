using ReservationSystem_Server.Data;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem_Server.Models.Reservation
{
    public class CreateVM
    {
        //start time, durationno. of guests, Notes/Add req
        public int SittingId { get; set; }
        public DateTime SittingStartTime { get; set; }   
        public DateTime SittingEndTime { get; set; }   

        public string SittingType { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get ; set; }

        [Range(0,1000)]
        public int NoOfPeople { get; set; }

        public string Notes { get; set; }


    }
}
