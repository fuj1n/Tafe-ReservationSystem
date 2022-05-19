using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class RestaurantCarouselVisual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestaurantCarouselItemVisuals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantCarouselItemVisuals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantCarouselItemVisuals_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RestaurantCarouselItemVisuals",
                columns: new[] { "Id", "ImageUrl", "RestaurantId", "Text" },
                values: new object[,]
                {
                    { 1, "~/images/home-carousel/ArabicaBeans.webp", 1, "We use only the finest Arabica beans" },
                    { 2, "~/images/home-carousel/BlueberryMuffin.webp", 1, "Freshly baked muffins everyday" },
                    { 3, "~/images/home-carousel/Cakes.webp", 1, "Scrumptous cakes freshly baked" },
                    { 4, "~/images/home-carousel/Cappuccino.webp", 1, "Fancy a cappuccino?" },
                    { 5, "~/images/home-carousel/Counter.webp", 1, "Our friendly baristas are ready to serve you" },
                    { 6, "~/images/home-carousel/FunctionArea.webp", 1, "We cater for large groups and functions" },
                    { 7, "~/images/home-carousel/MainArea.webp", 1, "Our café is ready to brighten up your morning" },
                    { 8, "~/images/home-carousel/OutdoorGarden.webp", 1, "Enjoy the fresh air in our outdoor garden" },
                    { 9, "~/images/home-carousel/ToastedSandwich2.webp", 1, "Try one of our famous grilled sandwiches" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantCarouselItemVisuals_RestaurantId",
                table: "RestaurantCarouselItemVisuals",
                column: "RestaurantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantCarouselItemVisuals");
        }
    }
}
