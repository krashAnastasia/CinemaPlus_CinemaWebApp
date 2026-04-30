using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaPlus.CinemaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedSharedTrailerForAllFilms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 1,
                column: "TrailerPath",
                value: "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov");

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 2,
                column: "TrailerPath",
                value: "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov");

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 3,
                column: "TrailerPath",
                value: "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov");

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 4,
                column: "TrailerPath",
                value: "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov");

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 5,
                column: "TrailerPath",
                value: "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov");

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 6,
                column: "TrailerPath",
                value: "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov");

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 7,
                column: "TrailerPath",
                value: "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov");

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 8,
                column: "TrailerPath",
                value: "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 1,
                column: "TrailerPath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 2,
                column: "TrailerPath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 3,
                column: "TrailerPath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 4,
                column: "TrailerPath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 5,
                column: "TrailerPath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 6,
                column: "TrailerPath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 7,
                column: "TrailerPath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 8,
                column: "TrailerPath",
                value: null);
        }
    }
}
