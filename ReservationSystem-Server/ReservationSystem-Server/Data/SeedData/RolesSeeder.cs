using Microsoft.AspNetCore.Identity;
using ReservationSystem_Server.Utility.DataSeed;

namespace ReservationSystem_Server.Data.SeedData;

public static class RolesSeeder
{
    [DataSeeder]
    private static void SeedData(List<IdentityRole> roles)
    {
        roles.Add(new IdentityRole
        {
                Id = "1337x01",
                Name = "Admin",
                NormalizedName = "ADMIN"
        });
        
        roles.Add(new IdentityRole
        {
                Id = "1337x02",
                Name = "Manager",
                NormalizedName = "MANAGER"
        });
        
        roles.Add(new IdentityRole
        {
                Id = "1337x03",
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
        });
        
        roles.Add(new IdentityRole
        {
                Id = "1337x04",
                Name = "Member",
                NormalizedName = "MEMBER"
        });
    }
}