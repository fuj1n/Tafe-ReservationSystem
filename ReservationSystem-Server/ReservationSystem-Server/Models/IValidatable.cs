using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ReservationSystem_Server.Models;

public interface IValidatable
{
    void Validate(ModelStateDictionary modelState);
}