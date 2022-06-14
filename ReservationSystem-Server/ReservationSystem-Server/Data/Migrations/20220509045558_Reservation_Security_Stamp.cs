using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class Reservation_Security_Stamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1337x03", "13da5cd2-2c3b-475f-82a6-f79b704b4ff7" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1337x03", "13da5cd2-2c3b-475f-82a6-f79b704b4ff7" });

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Reservations");
        }
    }
}
