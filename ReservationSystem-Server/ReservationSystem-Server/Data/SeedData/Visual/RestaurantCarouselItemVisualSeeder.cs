using ReservationSystem_Server.Data.Visual;
using ReservationSystem_Server.Helper.DataSeed;

namespace ReservationSystem_Server.Data.SeedData.Visual;

public static class RestaurantCarouselItemVisualSeeder
{
    private static readonly (string, string)[] Data =
    {
        ("ArabicaBeans", "We use only the finest Arabica beans"),
        ("BlueberryMuffin", "Freshly baked muffins everyday"),
        ("Cakes", "Scrumptous cakes freshly baked"),
        ("Cappuccino", "Fancy a cappuccino?"),
        ("Counter", "Our friendly baristas are ready to serve you"),
        ("FunctionArea", "We cater for large groups and functions"),
        ("MainArea", "Our café is ready to brighten up your morning"),
        ("OutdoorGarden", "Enjoy the fresh air in our outdoor garden"),
        ("ToastedSandwich2", "Try one of our famous grilled sandwiches")
    };
    
    [DataSeeder]
    private static void SeedData(List<RestaurantCarouselItemVisual> list)
    {
        int id = 1;
        
        list.AddRange(Data
            .Select(x => new RestaurantCarouselItemVisual
            {
                Id = id++,
                RestaurantId = 1,
                ImageUrl = $"~/images/home-carousel/{x.Item1}.webp",
                Text = x.Item2
            }));
    }
}