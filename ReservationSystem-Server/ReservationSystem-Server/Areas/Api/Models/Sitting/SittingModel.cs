using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using ReservationSystem_Server.Models;

namespace ReservationSystem_Server.Areas.Api.Models.Sitting;

[PublicAPI]
public class SittingModel : IOutputModel
{
    [Required]
    public int Id { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
    [Required]
    public bool IsClosed { get; set; }
    [Required]
    public int Capacity { get; set; }
    [Required]
    public int SittingTypeId { get; set; }

    [Required]
    public int RestaurantId { get; set; }
    public TimeSpan DefaultDuration { get; set; } = TimeSpan.FromMinutes(30);

    public LinkModel[] Links => new[]
    {
        new LinkModel($"sittings/types/{SittingTypeId}", "sitting_type"),
        new LinkModel($"restaurants/{RestaurantId}", "restaurant"),
        new LinkModel($"reservations/create/{Id}", "create_reservation", "POST"),
        new LinkModel($"sittings/timeSlots/{Id}", "time_slots")
    };

    public SittingModel FromSitting(Data.Sitting? sitting)
    {
        if (sitting == null)
            return this;
        
        Id = sitting.Id;
        StartTime = sitting.StartTime;
        EndTime = sitting.EndTime;
        IsClosed = sitting.IsClosed;
        Capacity = sitting.Capacity;
        SittingTypeId = sitting.SittingTypeId;
        RestaurantId = sitting.RestaurantId;
        DefaultDuration = sitting.DefaultDuration;

        return this;
    }

    public void ToSitting(Data.Sitting? sitting)
    {
        if (sitting == null)
            throw new InvalidOperationException("Attempted to modify a non-existing sitting");
        
        // Do not copy Id
        sitting.StartTime = StartTime;
        sitting.EndTime = EndTime;
        sitting.IsClosed = IsClosed;
        sitting.Capacity = Capacity;
        sitting.SittingTypeId = SittingTypeId;
        sitting.RestaurantId = RestaurantId;
        sitting.DefaultDuration = DefaultDuration;
    }
}