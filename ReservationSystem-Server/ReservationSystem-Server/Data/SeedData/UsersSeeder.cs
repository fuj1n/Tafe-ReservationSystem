using Microsoft.AspNetCore.Identity;
using ReservationSystem_Server.Utility.DataSeed;

namespace ReservationSystem_Server.Data.SeedData;

public static class UsersSeeder
{
    private static IdentityUser Construct(string id, string userName, string pass, string sec)
    {
        return new IdentityUser
        {
                Id = id,
                UserName = userName,
                NormalizedUserName = userName.ToUpper(),
                Email = userName,
                NormalizedEmail = userName.ToUpper(),
                PasswordHash = pass,
                SecurityStamp = sec,
                EmailConfirmed = true,
                ConcurrencyStamp = id
        };
    }

    [DataSeeder]
    private static void SeedData(List<IdentityUser> users)
    {
        users.AddRange(new[]
        {
                Construct("13da5cd2-2c3b-475f-82a6-f79b704b4ff7",
                        "a@e.com",
                        "AQAAAAEAACcQAAAAEHSUU32Zw7aBE34jWyoGCoBehbr6qSToqa89rWKoh9M105EwLGLm8L+mqBWnBar7+w==",
                        "KVZ7KVW5HBEBGXQKQHBQYJRSWX4UX35B"),
                Construct("4957f11c-e346-4abf-bc30-323e92eec3e8",
                        "m@e.com",
                        "AQAAAAEAACcQAAAAEA82JsRe5MpYglx5G1BJC51p/CcINT7Bo3k6/8RJIGJbtammuClPdbHpLhu6MThHnA==",
                        "PWKEKUHIGJJOPCUQSPMGVOVP7EX7G26K"),
                Construct("6e05e0c8-5ea0-4743-8d79-4512ff4c6068",
                        "e@e.com",
                        "AQAAAAEAACcQAAAAEL5843b/m8TQ3BBGvrLn4/wOs0/3s3gngaB5Hh9YHR3HohLbuFMIl5YIyEe+Lv9clg==",
                        "FEETQZYVJT2JVNFVJ27W5KBJPPUYXMK5"),
                Construct("de52760d-3485-4a2a-b892-4e389dfe44b8",
                        "u@e.com",
                        "AQAAAAEAACcQAAAAEOZVYYNMk47BIGvZeyLotCWuc6xyzI8lBEkEEYGZp+6CLgbyV9Vu+Wj/nPiBzI74pg==",
                        "TUVBKEMQFRVMYC3UNIZAHCUV4JWLDQKP"),
                
        });
    }
}