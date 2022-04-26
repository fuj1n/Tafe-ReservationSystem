using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReservationSystem_Server.Areas.Admin.Models.Sitting
{
    public class CreateVM
    {
        [ValidateNever]
        public DateTime StartTime { get; set; }
        [ValidateNever]
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        public int SittingType { get; set; }
        public SelectList? SittingTypes { get; set; }
    }
}
