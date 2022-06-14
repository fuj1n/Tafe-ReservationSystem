using ReservationSystem_Server.Helper.DataSeed;

namespace ReservationSystem_Server.Data.SeedData;

public static class RestaurantAreaSeeder
{
    [DataSeeder]
    private static void SeedData(List<RestaurantArea> areas)
    {
        areas.AddRange(
                new[]
                {
                        new RestaurantArea
                        {
                                Id = 1,
                                Name = "Main",
                                RestaurantId = 1
                        },
                        new RestaurantArea
                        {
                                        Id = 2,
                                        Name = "Outside",
                                        RestaurantId = 1
                        },
                        new RestaurantArea
                        {
                                Id = 3,
                                Name = "Balcony",
                                RestaurantId = 1
                        }
                }
        );
    }
}