using ReservationSystem_Server.Utility.DataSeed;

namespace ReservationSystem_Server.Data.SeedData;

public static class ReservationOriginSeeder
{
    [DataSeeder]
    private static void SeedData(List<ReservationOrigin> reservationOrigins)
    {
        reservationOrigins.AddRange(new []
        {
                new ReservationOrigin { Id = 1, Description = "In-Person" },
                new ReservationOrigin { Id = 2, Description = "Email" },
                new ReservationOrigin { Id = 3, Description = "Phone" },
                new ReservationOrigin { Id = 4, Description = "Online" },
                new ReservationOrigin { Id = 5, Description = "Mobile" }
        });
    }
}