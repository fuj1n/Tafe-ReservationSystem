using ReservationSystem_Server.Helper.DataSeed;

namespace ReservationSystem_Server.Data.SeedData;

public static class SittingSeeder
{
    [DataSeeder]
    private static void SeedData(List<Sitting> sittings)
    {
        sittings.AddRange(
                new[]
                {
                        new Sitting
                        {
                                Id = 1,
                                StartTime = new DateTime(2022, 03, 31, 6, 0, 0, DateTimeKind.Local),
                                EndTime = new DateTime(2022, 03, 31, 9, 0, 0, DateTimeKind.Local),
                                RestaurantId = 1,
                                SittingTypeId = 1
                        },
                        new Sitting
                        {
                                Id = 2,
                                StartTime = new DateTime(2022, 03, 31, 11, 0, 0, DateTimeKind.Local),
                                EndTime = new DateTime(2022, 03, 31, 14, 0, 0, DateTimeKind.Local),
                                RestaurantId = 1,
                                SittingTypeId = 2
                        },
                        new Sitting
                        {
                                Id = 3,
                                StartTime = new DateTime(2022, 03, 31, 17, 0, 0, DateTimeKind.Local),
                                EndTime = new DateTime(2022, 03, 31, 21, 0, 0, DateTimeKind.Local),
                                RestaurantId = 1,
                                SittingTypeId = 3
                        }
                });
    }
}