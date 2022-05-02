namespace ReservationSystem_Server.Models;

public class StatusCodeErrorViewModel
{
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorUrl { get; set; }
}