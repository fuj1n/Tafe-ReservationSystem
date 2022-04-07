using ReservationSystem_Server.Utility.DataSeed;

namespace ReservationSystem_Server.Data.SeedData;

public static class TableSeeder
{
    [DataSeeder]
    private static void SeedData(List<Table> tables)
    {
        char[] areas = { 'M', 'O', 'B' };
        int acc = 1;
        for (int a = 1; a <= areas.Length; a++)
        {
            char area = areas[a - 1];
            for (int i = 1; i <= 10; i++)
            {
                tables.Add(new Table
                {
                        Id = ++acc,
                        Name = $"{area}{i}",
                        Seats = 4,
                        AreaId = a
                });
            }
        }
    }
}