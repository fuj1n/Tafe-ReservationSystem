using ReservationSystem_Server.Data.Visual;
using ReservationSystem_Server.Utility.DataSeed;

namespace ReservationSystem_Server.Data.SeedData.Visual;

public static class ReservationStatusVisualSeeder
{
    [DataSeeder]
    private static void SeedData(List<ReservationStatusVisual> visuals)
    {
        visuals.AddRange(new[]
        {
            new ReservationStatusVisual {Id = 1, HtmlBadgeClass = "bg-warning"}, // Pending
            new ReservationStatusVisual {Id = 2, HtmlBadgeClass = "bg-success"}, // Confirmed
            new ReservationStatusVisual {Id = 3, HtmlBadgeClass = "bg-danger"}, // Cancelled
            new ReservationStatusVisual {Id = 4, HtmlBadgeClass = "bg-info"}, // Seated
            new ReservationStatusVisual {Id = 5, HtmlBadgeClass = "bg-dark"}, // Completed
        });
    }
}