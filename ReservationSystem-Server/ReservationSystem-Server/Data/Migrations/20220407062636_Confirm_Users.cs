using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class Confirm_Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x01",
                column: "ConcurrencyStamp",
                value: "e1fec68e-64ee-4aa8-b823-5a5a0bc0b981");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x02",
                column: "ConcurrencyStamp",
                value: "daddf9b9-ca5e-4aa7-b6c1-5b628abe9854");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x03",
                column: "ConcurrencyStamp",
                value: "9ce781b0-ae68-4588-81a0-e1577c6386bd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x04",
                column: "ConcurrencyStamp",
                value: "09d8290e-d527-4796-88e3-b83956ddb170");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13da5cd2-2c3b-475f-82a6-f79b704b4ff7",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed" },
                values: new object[] { "3e25ee09-9515-4ff2-a146-71b728fae1d2", true });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4957f11c-e346-4abf-bc30-323e92eec3e8",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed" },
                values: new object[] { "0d4ba88c-217a-49d5-8299-604b8f2134a4", true });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e05e0c8-5ea0-4743-8d79-4512ff4c6068",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed" },
                values: new object[] { "a977365e-3432-4ec3-acfa-fae63f9a143b", true });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "de52760d-3485-4a2a-b892-4e389dfe44b8",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed" },
                values: new object[] { "207f5598-4796-41a9-85a0-8e7541de98ec", true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x01",
                column: "ConcurrencyStamp",
                value: "eab7975c-a0a7-4f6f-929a-1a9cd8f06bd7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x02",
                column: "ConcurrencyStamp",
                value: "c62e3a0e-a93f-4ffe-bba4-ac9e7b2fac5b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x03",
                column: "ConcurrencyStamp",
                value: "3e5f99e4-b566-4312-be3d-2ad716e5927c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x04",
                column: "ConcurrencyStamp",
                value: "352d7bad-58ec-4a85-be8c-46226806e928");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13da5cd2-2c3b-475f-82a6-f79b704b4ff7",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed" },
                values: new object[] { "0b8c5f3c-89df-41f9-92e1-d1eed4bfbd0d", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4957f11c-e346-4abf-bc30-323e92eec3e8",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed" },
                values: new object[] { "5ed5eeb4-e1f8-47c3-89e8-c5911a55fb01", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e05e0c8-5ea0-4743-8d79-4512ff4c6068",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed" },
                values: new object[] { "64f99018-ba77-4475-8719-406d2704f734", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "de52760d-3485-4a2a-b892-4e389dfe44b8",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed" },
                values: new object[] { "a008be0c-9431-41dc-878f-ad6db6139e9c", false });
        }
    }
}
