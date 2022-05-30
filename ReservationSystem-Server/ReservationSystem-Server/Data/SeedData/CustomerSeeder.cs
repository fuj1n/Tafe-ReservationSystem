using ReservationSystem_Server.Helper.DataSeed;

namespace ReservationSystem_Server.Data.SeedData;

public static class CustomerSeeder
{
    [DataSeeder]
    private static void SeedData(List<Customer> customers)
    {
        customers.Add(new Customer
        {
            Id = 1,
            UserId = "de52760d-3485-4a2a-b892-4e389dfe44b8",
            FirstName = "John",
            LastName = "Doe",
            Email = "U@E.COM",
            PhoneNumber = "0412345678"
        });
    }
}