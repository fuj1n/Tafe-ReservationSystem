using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class ReservationStatusVisual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservationStatusVisuals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    HtmlBadgeClass = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationStatusVisuals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationStatusVisuals_ReservationStatuses_Id",
                        column: x => x.Id,
                        principalTable: "ReservationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x01",
                column: "ConcurrencyStamp",
                value: "1337x01");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x02",
                column: "ConcurrencyStamp",
                value: "1337x02");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x03",
                column: "ConcurrencyStamp",
                value: "1337x03");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1337x04",
                column: "ConcurrencyStamp",
                value: "1337x04");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13da5cd2-2c3b-475f-82a6-f79b704b4ff7",
                column: "ConcurrencyStamp",
                value: "13da5cd2-2c3b-475f-82a6-f79b704b4ff7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4957f11c-e346-4abf-bc30-323e92eec3e8",
                column: "ConcurrencyStamp",
                value: "4957f11c-e346-4abf-bc30-323e92eec3e8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e05e0c8-5ea0-4743-8d79-4512ff4c6068",
                column: "ConcurrencyStamp",
                value: "6e05e0c8-5ea0-4743-8d79-4512ff4c6068");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "de52760d-3485-4a2a-b892-4e389dfe44b8",
                column: "ConcurrencyStamp",
                value: "de52760d-3485-4a2a-b892-4e389dfe44b8");

            migrationBuilder.InsertData(
                table: "ReservationStatusVisuals",
                columns: new[] { "Id", "HtmlBadgeClass" },
                values: new object[,]
                {
                    { 1, "bg-warning" },
                    { 2, "bg-success" },
                    { 3, "bg-danger" },
                    { 4, "bg-info" },
                    { 5, "bg-dark" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationStatusVisuals");

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
                column: "ConcurrencyStamp",
                value: "3e25ee09-9515-4ff2-a146-71b728fae1d2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4957f11c-e346-4abf-bc30-323e92eec3e8",
                column: "ConcurrencyStamp",
                value: "0d4ba88c-217a-49d5-8299-604b8f2134a4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e05e0c8-5ea0-4743-8d79-4512ff4c6068",
                column: "ConcurrencyStamp",
                value: "a977365e-3432-4ec3-acfa-fae63f9a143b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "de52760d-3485-4a2a-b892-4e389dfe44b8",
                column: "ConcurrencyStamp",
                value: "207f5598-4796-41a9-85a0-8e7541de98ec");
        }
    }
}
