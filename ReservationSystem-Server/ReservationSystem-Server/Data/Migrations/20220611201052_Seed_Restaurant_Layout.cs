using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationSystem_Server.Migrations
{
    public partial class Seed_Restaurant_Layout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RectangleVisuals",
                columns: new[] { "Id", "A", "B", "G", "Height", "R", "TableTypeVisualId", "Width", "X", "Y" },
                values: new object[,]
                {
                    { 1, (byte)255, (byte)254, (byte)19, 25f, (byte)144, null, 25f, 48.241886f, 34.69217f },
                    { 2, (byte)255, (byte)33, (byte)211, 25f, (byte)126, null, 25f, 48.138847f, 9.550489f },
                    { 3, (byte)255, (byte)194, (byte)227, 37.986217f, (byte)80, null, 43.75644f, 4.620685f, 9.3089905f }
                });

            migrationBuilder.InsertData(
                table: "TableTypeVisuals",
                columns: new[] { "Id", "Height", "Name", "Seats", "Width" },
                values: new object[] { 1, 7f, "2-seater", 2, 5f });

            migrationBuilder.InsertData(
                table: "RectangleVisuals",
                columns: new[] { "Id", "A", "B", "G", "Height", "R", "TableTypeVisualId", "Width", "X", "Y" },
                values: new object[,]
                {
                    { 4, (byte)255, (byte)42, (byte)87, 89.64534f, (byte)139, 1, 99.94848f, 0.051519834f, 5.225631f },
                    { 5, (byte)255, (byte)27, (byte)2, 9.997585f, (byte)208, 1, 86.17336f, 6.7168984f, 0.012075072f },
                    { 6, (byte)255, (byte)27, (byte)2, 10.218387f, (byte)208, 1, 86.37944f, 7.9694743f, 89.76178f }
                });

            migrationBuilder.InsertData(
                table: "RestaurantAreaVisuals",
                columns: new[] { "AreaId", "RectId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "TableVisuals",
                columns: new[] { "TableId", "Rotation", "TableTypeId", "X", "Y" },
                values: new object[,]
                {
                    { 2, 0, 1, 47.552807f, 39.464195f },
                    { 3, 0, 1, 73.20969f, 69.44874f },
                    { 4, 0, 1, 39.92787f, 70.47913f },
                    { 5, 0, 1, 3.5548687f, 70.47913f },
                    { 6, 0, 1, 71.04585f, 38.12468f },
                    { 7, 0, 1, 26.120556f, 39.567234f },
                    { 8, 0, 1, 2.833591f, 38.22772f },
                    { 9, 0, 1, 70.32458f, 1.7516744f },
                    { 10, 0, 1, 38.691395f, 1.9577538f },
                    { 11, 0, 1, 3.0396702f, 2.1638331f },
                    { 12, 0, 1, 54.147346f, 35.548687f },
                    { 13, 0, 1, 31.478619f, 36.166924f },
                    { 14, 90, 1, 40.75219f, 71.92169f },
                    { 15, 90, 1, 41.267387f, 0.61823803f },
                    { 16, 0, 1, 77.43431f, 35.651726f },
                    { 17, 0, 1, 6.7490983f, 36.269962f },
                    { 18, 135, 1, 6.7490983f, 69.963936f },
                    { 19, 45, 1, 73.724884f, 69.654816f },
                    { 20, 135, 1, 73.724884f, 2.5759919f },
                    { 21, 45, 1, 4.997424f, 2.5759919f },
                    { 22, 90, 1, 23.441525f, 36.438354f },
                    { 23, 0, 1, 77.53735f, 72.75802f },
                    { 24, 0, 1, 77.022156f, 36.67574f },
                    { 25, 0, 1, 77.12519f, 5.459819f },
                    { 26, 0, 1, 43.1221f, 70.146805f },
                    { 27, 0, 1, 42.916023f, 35.607513f },
                    { 28, 0, 1, 43.94642f, 4.86636f },
                    { 29, 0, 1, 4.585265f, 68.485115f },
                    { 30, 0, 1, 4.379186f, 34.539288f },
                    { 31, 0, 1, 3.5548687f, 4.035518f }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RectangleVisuals",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RectangleVisuals",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RectangleVisuals",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RestaurantAreaVisuals",
                keyColumn: "AreaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RestaurantAreaVisuals",
                keyColumn: "AreaId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RestaurantAreaVisuals",
                keyColumn: "AreaId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "TableVisuals",
                keyColumn: "TableId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "RectangleVisuals",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RectangleVisuals",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RectangleVisuals",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TableTypeVisuals",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
