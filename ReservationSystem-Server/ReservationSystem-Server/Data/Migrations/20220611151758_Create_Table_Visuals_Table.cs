using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class Create_Table_Visuals_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TableVisuals",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false),
                    TableTypeId = table.Column<int>(type: "int", nullable: false),
                    X = table.Column<float>(type: "real", nullable: false),
                    Y = table.Column<float>(type: "real", nullable: false),
                    Rotation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableVisuals", x => x.TableId);
                    table.ForeignKey(
                        name: "FK_TableVisuals_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableVisuals_TableTypeVisuals_TableTypeId",
                        column: x => x.TableTypeId,
                        principalTable: "TableTypeVisuals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TableVisuals_TableTypeId",
                table: "TableVisuals",
                column: "TableTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableVisuals");
        }
    }
}
