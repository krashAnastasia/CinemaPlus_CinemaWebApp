using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaPlus.CinemaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMenInBlackToMainPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Films",
                columns: new[] { "Id", "AgeRestriction", "AvailabilityDate", "AvailabilityStatus", "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title", "TrailerPath" },
                values: new object[] { 18, "12+", new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "NowShowing", "Таємна організація захищає Землю від міжгалактичних загроз, а новачок проходить випробування у світі прибульців і великих секретів.", 98, "Фантастика", "/source/logo-white.png", 1997, "Люди в чорному", "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov" });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "FilmId", "HallId", "Price", "SessionTime" },
                values: new object[,]
                {
                    { 270, 18, 1, 175m, new DateTime(2026, 7, 8, 10, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 271, 18, 2, 215m, new DateTime(2026, 7, 8, 14, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 272, 18, 3, 255m, new DateTime(2026, 7, 8, 19, 35, 0, 0, DateTimeKind.Unspecified) },
                    { 273, 18, 1, 175m, new DateTime(2026, 7, 11, 10, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 274, 18, 2, 215m, new DateTime(2026, 7, 11, 14, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 275, 18, 3, 255m, new DateTime(2026, 7, 11, 19, 35, 0, 0, DateTimeKind.Unspecified) },
                    { 276, 18, 1, 175m, new DateTime(2026, 7, 14, 10, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 277, 18, 2, 215m, new DateTime(2026, 7, 14, 14, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 278, 18, 3, 255m, new DateTime(2026, 7, 14, 19, 35, 0, 0, DateTimeKind.Unspecified) },
                    { 279, 18, 1, 175m, new DateTime(2026, 7, 17, 10, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 280, 18, 2, 215m, new DateTime(2026, 7, 17, 14, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 281, 18, 3, 255m, new DateTime(2026, 7, 17, 19, 35, 0, 0, DateTimeKind.Unspecified) },
                    { 282, 18, 1, 175m, new DateTime(2026, 7, 20, 10, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 283, 18, 2, 215m, new DateTime(2026, 7, 20, 14, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 284, 18, 3, 255m, new DateTime(2026, 7, 20, 19, 35, 0, 0, DateTimeKind.Unspecified) },
                    { 285, 18, 1, 175m, new DateTime(2026, 7, 23, 10, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 286, 18, 2, 215m, new DateTime(2026, 7, 23, 14, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 287, 18, 3, 255m, new DateTime(2026, 7, 23, 19, 35, 0, 0, DateTimeKind.Unspecified) },
                    { 288, 18, 1, 175m, new DateTime(2026, 7, 26, 10, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 289, 18, 2, 215m, new DateTime(2026, 7, 26, 14, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 290, 18, 3, 255m, new DateTime(2026, 7, 26, 19, 35, 0, 0, DateTimeKind.Unspecified) },
                    { 291, 18, 1, 175m, new DateTime(2026, 7, 29, 10, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 292, 18, 2, 215m, new DateTime(2026, 7, 29, 14, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 293, 18, 3, 255m, new DateTime(2026, 7, 29, 19, 35, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 271);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 272);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 273);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 274);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 275);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 276);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 277);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 278);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 279);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 280);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 281);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 282);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 283);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 284);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 285);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 286);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 287);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 288);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 289);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 290);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 291);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 292);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 293);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 18);
        }
    }
}
