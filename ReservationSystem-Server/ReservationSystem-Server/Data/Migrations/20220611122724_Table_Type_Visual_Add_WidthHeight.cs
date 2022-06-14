using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class Table_Type_Visual_Add_WidthHeight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "TableTypeVisuals",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Width",
                table: "TableTypeVisuals",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "TableTypeVisuals");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "TableTypeVisuals");
        }
    }
}
