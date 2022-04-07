using ReservationSystem_Server.Utility.DataSeed;

namespace ReservationSystem_Server.Data.SeedData;

public static class ReservationStatusSeeder
{
    [DataSeeder]
    private static void SeedData(List<ReservationStatus> reservationStatuses)
    {
        reservationStatuses.AddRange(new[]
        {
            new ReservationStatus { Id = 1, Description = "Pending" },
            new ReservationStatus { Id = 2, Description = "Confirmed" },
            new ReservationStatus { Id = 3, Description = "Canceled" },
            new ReservationStatus { Id = 4, Description = "Seated" },
            new ReservationStatus { Id = 5, Description = "Completed" }
        });
    }
}