using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class UpdateReservationStatusDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ReservationStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Cancelled");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ReservationStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Canceled");
        }
    }
}
