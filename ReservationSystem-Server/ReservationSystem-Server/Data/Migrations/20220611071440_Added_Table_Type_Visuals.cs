using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class Added_Table_Type_Visuals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TableTypeVisualId",
                table: "RectangleVisuals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TableTypeVisuals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTypeVisuals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RectangleVisuals_TableTypeVisualId",
                table: "RectangleVisuals",
                column: "TableTypeVisualId");

            migrationBuilder.AddForeignKey(
                name: "FK_RectangleVisuals_TableTypeVisuals_TableTypeVisualId",
                table: "RectangleVisuals",
                column: "TableTypeVisualId",
                principalTable: "TableTypeVisuals",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RectangleVisuals_TableTypeVisuals_TableTypeVisualId",
                table: "RectangleVisuals");

            migrationBuilder.DropTable(
                name: "TableTypeVisuals");

            migrationBuilder.DropIndex(
                name: "IX_RectangleVisuals_TableTypeVisualId",
                table: "RectangleVisuals");

            migrationBuilder.DropColumn(
                name: "TableTypeVisualId",
                table: "RectangleVisuals");
        }
    }
}
