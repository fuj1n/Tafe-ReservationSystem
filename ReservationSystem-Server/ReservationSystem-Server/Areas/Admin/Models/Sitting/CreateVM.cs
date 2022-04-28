using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem_Server.Areas.Admin.Models.Sitting
{
    public class CreateVM
    {
        //[ValidateNever]
        public DateTime StartTime { get; set; }
        //[ValidateNever]
        public DateTime EndTime { get; set; }
        [Range(0, 10000)]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Please select a sitting type")]
        public int SittingType { get; set; }
        public SelectList? SittingTypes { get; set; }
    }
}
