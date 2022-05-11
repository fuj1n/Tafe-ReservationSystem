using ReservationSystem_Server.Areas.Api.Models;

namespace ReservationSystem_Server.Models;

public interface IOutputModel
{
    LinkModel[] Links { get; }
}