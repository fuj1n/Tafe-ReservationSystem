namespace ReservationSystem_Server.Data.Visual;

public class RestaurantCarouselItemVisual
{
    public int Id { get; set; }
    
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    
    public string ImageUrl { get; set; } = null!;
    public string Text { get; set; } = null!;
}