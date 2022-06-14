using JetBrains.Annotations;
using ReservationSystem_Server.Models;

namespace ReservationSystem_Server.Areas.Api.Models.Reservation.Admin;

[PublicAPI]
public class ReservationModel : CreateModel, IOutputModel
{
    public int Id { get; set; }
    public int ReservationStatusId { get; set; }

    public LinkModel[] Links => new[]
    {
        new LinkModel($"reservation/{Id}", "self"),
        new LinkModel($"reservation/{Id}/edit", "edit", "POST"),
        new LinkModel($"reservation/origin/{ReservationOriginId}", "origin"),
        new LinkModel($"reservation/status/{ReservationStatusId}", "status"),
        new LinkModel($"reservation/{Id}/status", "set_status", "POST")
    };

    public static ReservationModel FromReservation(Data.Reservation reservation)
    {
        return new ReservationModel
        {
            Id = reservation.Id,
            SittingId = reservation.SittingId,
            Customer = new CustomerModel
            {
                FirstName = reservation.Customer.FirstName,
                LastName = reservation.Customer.LastName,
                Email = reservation.Customer.Email,
                PhoneNumber = reservation.Customer.PhoneNumber
            },
            StartTime = reservation.StartTime,
            Duration = reservation.Duration,
            ReservationOriginId = reservation.ReservationOriginId,
            ReservationStatusId = reservation.ReservationStatusId,
            NumberOfGuests = reservation.NumberOfPeople,
            Notes = reservation.Notes,
        };
    }
}