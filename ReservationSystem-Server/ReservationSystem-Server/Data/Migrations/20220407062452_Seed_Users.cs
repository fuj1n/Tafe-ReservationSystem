using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class Seed_Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1337x01", "eab7975c-a0a7-4f6f-929a-1a9cd8f06bd7", "Admin", "ADMIN" },
                    { "1337x02", "c62e3a0e-a93f-4ffe-bba4-ac9e7b2fac5b", "Manager", "MANAGER" },
                    { "1337x03", "3e5f99e4-b566-4312-be3d-2ad716e5927c", "Employee", "EMPLOYEE" },
                    { "1337x04", "352d7bad-58ec-4a85-be8c-46226806e928", "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "13da5cd2-2c3b-475f-82a6-f79b704b4ff7", 0, "0b8c5f3c-89df-41f9-92e1-d1eed4bfbd0d", "a@e.com", false, false, null, "A@E.COM", "A@E.COM", "AQAAAAEAACcQAAAAEHSUU32Zw7aBE34jWyoGCoBehbr6qSToqa89rWKoh9M105EwLGLm8L+mqBWnBar7+w==", null, false, "KVZ7KVW5HBEBGXQKQHBQYJRSWX4UX35B", false, "a@e.com" },
                    { "4957f11c-e346-4abf-bc30-323e92eec3e8", 0, "5ed5eeb4-e1f8-47c3-89e8-c5911a55fb01", "m@e.com", false, false, null, "M@E.COM", "M@E.COM", "AQAAAAEAACcQAAAAEA82JsRe5MpYglx5G1BJC51p/CcINT7Bo3k6/8RJIGJbtammuClPdbHpLhu6MThHnA==", null, false, "PWKEKUHIGJJOPCUQSPMGVOVP7EX7G26K", false, "m@e.com" },
                    { "6e05e0c8-5ea0-4743-8d79-4512ff4c6068", 0, "64f99018-ba77-4475-8719-406d2704f734", "e@e.com", false, false, null, "E@E.COM", "E@E.COM", "AQAAAAEAACcQAAAAEL5843b/m8TQ3BBGvrLn4/wOs0/3s3gngaB5Hh9YHR3HohLbuFMIl5YIyEe+Lv9clg==", null, false, "FEETQZYVJT2JVNFVJ27W5KBJPPUYXMK5", false, "e@e.com" },
                    { "de52760d-3485-4a2a-b892-4e389dfe44b8", 0, "a008be0c-9431-41dc-878f-ad6db6139e9c", "u@e.com", false, false, null, "U@E.COM", "U@E.COM", "AQAAAAEAACcQAAAAEOZVYYNMk47BIGvZeyLotCWuc6xyzI8lBEkEEYGZp+6CLgbyV9Vu+Wj/nPiBzI74pg==", null, false, "TUVBKEMQFRVMYC3UNIZAHCUV4JWLDQKP", false, "u@e.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1337x01", "13da5cd2-2c3b-475f-82a6-f79b704b4ff7" },
                    { "1337x02", "4957f11c-e346-4abf-bc30-323e92eec3e8" },
                    { "1337x03", "4957f11c-e346-4abf-bc30-323e92eec3e8" },
                    { "1337x03", "6e05e0c8-5ea0-4743-8d79-4512ff4c6068" },
                    { "1337x04", "de52760d-3485-4a2a-b892-4e389dfe44b8" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1337x01", "13da5cd2-2c3b-475f-82a6-f79b704b4ff7" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1337x02", "4957f11c-e346-4abf-bc30-323e92eec3e8" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1337x03", "4957f11c-e346-4abf-bc30-323e92eec3e8" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1337x03", "6e05e0c8-5ea0-4743-8d79-4512ff4c6068" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1337x04", "de52760d-3485-4a2a-b892-4e389dfe44b8" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x01");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x02");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x03");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x04");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13da5cd2-2c3b-475f-82a6-f79b704b4ff7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4957f11c-e346-4abf-bc30-323e92eec3e8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e05e0c8-5ea0-4743-8d79-4512ff4c6068");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "de52760d-3485-4a2a-b892-4e389dfe44b8");
        }
    }
}
