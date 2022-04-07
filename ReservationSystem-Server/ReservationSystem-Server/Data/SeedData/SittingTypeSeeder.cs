using ReservationSystem_Server.Utility.DataSeed;

namespace ReservationSystem_Server.Data.SeedData;

public static class SittingTypeSeeder
{
    [DataSeeder]
    private static void SeedData(List<SittingType> sittingTypes)
    {
        sittingTypes.AddRange(new []
        {
                new SittingType
                {
                        Id = 1,
                        Description = "Breakfast"
                },
                new SittingType
                {
                        Id = 2,
                        Description = "Lunch"
                },
                new SittingType
                {
                        Id = 3,
                        Description = "Dinner"
                },
        });
    }
}