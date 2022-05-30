using ReservationSystem_Server.Data.Visual;
using ReservationSystem_Server.Helper.DataSeed;

namespace ReservationSystem_Server.Data.SeedData.Visual;

public static class ReservationStatusVisualSeeder
{
    [DataSeeder]
    private static void SeedData(List<ReservationStatusVisual> visuals)
    {
        visuals.AddRange(new[]
        {
            new ReservationStatusVisual {Id = 1, HtmlBadgeClass = "bg-warning", ReactBadgeVariant = "warning"}, // Pending
            new ReservationStatusVisual {Id = 2, HtmlBadgeClass = "bg-success", ReactBadgeVariant = "success"}, // Confirmed
            new ReservationStatusVisual {Id = 3, HtmlBadgeClass = "bg-danger", ReactBadgeVariant = "danger"}, // Cancelled
            new ReservationStatusVisual {Id = 4, HtmlBadgeClass = "bg-info", ReactBadgeVariant = "info"}, // Seated
            new ReservationStatusVisual {Id = 5, HtmlBadgeClass = "bg-dark", ReactBadgeVariant = "dark"}, // Completed
        });
    }
}