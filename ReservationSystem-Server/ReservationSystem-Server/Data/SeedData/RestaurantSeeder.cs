using ReservationSystem_Server.Helper.DataSeed;

namespace ReservationSystem_Server.Data.SeedData;

public static class RestaurantSeeder
{
    [DataSeeder]
    private static void SeedData(List<Restaurant> restaurants)
    {
        restaurants.AddRange(new[]
        {
                new Restaurant
                {
                        Id = 1,
                        Name = "Bean Scene Café",
                        Address = "123 Main Street",
                        PhoneNumber = "0412345678",
                        Email = "email@example.com"
                },
        });
    }
}