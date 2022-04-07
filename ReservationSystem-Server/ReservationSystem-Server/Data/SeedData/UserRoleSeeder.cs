using Microsoft.AspNetCore.Identity;
using ReservationSystem_Server.Utility.DataSeed;

namespace ReservationSystem_Server.Data.SeedData;

public static class UserRoleSeeder
{
    [DataSeeder]
    private static void SeedData(List<IdentityUserRole<string>> userRoles)
    {
        userRoles.Add(new IdentityUserRole<string>
        {
                UserId = "13da5cd2-2c3b-475f-82a6-f79b704b4ff7", // a@e.com
                RoleId = "1337x01" // Admin
        });
        userRoles.Add(new IdentityUserRole<string>
        {
                UserId = "4957f11c-e346-4abf-bc30-323e92eec3e8", // m@e.com
                RoleId = "1337x02" // Manager
        });
        userRoles.Add(new IdentityUserRole<string>
        {
                UserId = "4957f11c-e346-4abf-bc30-323e92eec3e8", // m@e.com
                RoleId = "1337x03" // Employee
        });
        userRoles.Add(new IdentityUserRole<string>
        {
                UserId = "6e05e0c8-5ea0-4743-8d79-4512ff4c6068", // e@e.com
                RoleId = "1337x03" // Employee
        });
        userRoles.Add(new IdentityUserRole<string>
        {
                UserId = "de52760d-3485-4a2a-b892-4e389dfe44b8", // u@e.com
                RoleId = "1337x04" // Member
        });
    }
}