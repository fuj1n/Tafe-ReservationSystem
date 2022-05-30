using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class ReservationStatusVisual_ReactBadge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReactBadgeVariant",
                table: "ReservationStatusVisuals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ReservationStatusVisuals",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReactBadgeVariant",
                value: "warning");

            migrationBuilder.UpdateData(
                table: "ReservationStatusVisuals",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReactBadgeVariant",
                value: "success");

            migrationBuilder.UpdateData(
                table: "ReservationStatusVisuals",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReactBadgeVariant",
                value: "danger");

            migrationBuilder.UpdateData(
                table: "ReservationStatusVisuals",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReactBadgeVariant",
                value: "info");

            migrationBuilder.UpdateData(
                table: "ReservationStatusVisuals",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReactBadgeVariant",
                value: "dark");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReactBadgeVariant",
                table: "ReservationStatusVisuals");
        }
    }
}
