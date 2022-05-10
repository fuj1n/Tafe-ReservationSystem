namespace ReservationSystem_Server.Areas.Api.Models;

public class LinkModel
{
    public string Href { get; }
    public string Rel { get; }
    public string Type { get; }
    
    public LinkModel(string href, string rel, string type = "GET")
    {
        Href = href;
        Rel = rel;
        Type = type;
    }
}