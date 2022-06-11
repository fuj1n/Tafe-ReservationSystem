using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class Create_Restaurant_Area_Visual_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RectangleVisuals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<float>(type: "real", nullable: false),
                    Y = table.Column<float>(type: "real", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    R = table.Column<byte>(type: "tinyint", nullable: false),
                    G = table.Column<byte>(type: "tinyint", nullable: false),
                    B = table.Column<byte>(type: "tinyint", nullable: false),
                    A = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RectangleVisuals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantAreaVisuals",
                columns: table => new
                {
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    RectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantAreaVisuals", x => x.AreaId);
                    table.ForeignKey(
                        name: "FK_RestaurantAreaVisuals_RectangleVisuals_RectId",
                        column: x => x.RectId,
                        principalTable: "RectangleVisuals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantAreaVisuals_RestaurantAreas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "RestaurantAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantAreaVisuals_RectId",
                table: "RestaurantAreaVisuals",
                column: "RectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantAreaVisuals");

            migrationBuilder.DropTable(
                name: "RectangleVisuals");
        }
    }
}
