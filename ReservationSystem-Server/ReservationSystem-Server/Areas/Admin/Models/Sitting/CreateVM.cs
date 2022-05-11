using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public int SittingTypeId { get; set; }
        public SelectList? SittingTypes { get; set; }

        public void Validate(ModelStateDictionary state)
        {
            if (StartTime < DateTime.Now)
            {
                state.AddModelError("StartTime", "Start Time must be in the future");
            }

            if (EndTime <= StartTime)
            {
                state.AddModelError("EndTime", "End Time must be after Start Time");
            }
        }
    }
}
