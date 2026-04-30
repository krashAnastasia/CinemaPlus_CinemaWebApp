using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaPlus.CinemaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class ExpandJulyCatalogueAndAuthEmulation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 1,
                column: "AvailabilityDate",
                value: new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 2,
                column: "AvailabilityDate",
                value: new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 3,
                column: "AvailabilityDate",
                value: new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AvailabilityDate", "Title" },
                values: new object[] { new DateTime(2026, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Матриця" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 5,
                column: "AvailabilityDate",
                value: new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AvailabilityDate", "Title" },
                values: new object[] { new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Месники" });

            migrationBuilder.InsertData(
                table: "Films",
                columns: new[] { "Id", "AgeRestriction", "AvailabilityDate", "AvailabilityStatus", "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title", "TrailerPath" },
                values: new object[,]
                {
                    { 9, "12+", new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "NowShowing", "Пітер Паркер намагається втримати баланс між героїзмом, почуттями та новою темною силою, що загрожує місту.", 139, "Екшн", "/source/avengers-poster.jpg", 2007, "Людина-павук 3", "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov" },
                    { 10, "12+", new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "NowShowing", "Нова ера парку динозаврів виходить з-під контролю, і відвідувачам доводиться боротися за виживання.", 124, "Пригоди", "/source/hero-img.jpg", 2015, "Світ Юрського періоду", "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov" },
                    { 11, "12+", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "NowShowing", "Белла Свон переїжджає до Форкса та знайомиться з Едвардом Калленом, що змінює її життя назавжди.", 122, "Романтика", "/source/form-hero.png", 2008, "Сутінки", "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov" },
                    { 12, "0+", new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "NowShowing", "Джуді Гопс і Нік Вайлд повертаються з новою справою, яка випробує їхню дружбу та спритність.", 108, "Анімація", "/source/logo-blue.png", 2025, "Зоотрополіс 2", "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov" },
                    { 13, "16+", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "NowShowing", "Темний лицар повертається до Ґотема, щоб розслідувати серію злочинів і зупинити нову хвилю хаосу.", 176, "Екшн", "/source/cinema-logo.png", 2024, "Бетмен (2024)", "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov" },
                    { 14, "12+", new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "ComingSoon", "Історія доброго та щирого Форреста, чий незвичайний життєвий шлях проходить крізь найяскравіші події епохи.", 142, "Драма", "/source/hero-img.jpg", 1994, "Форрест Ґамп", "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov" },
                    { 15, "12+", new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "ComingSoon", "Група дослідників вирушає крізь червоточину, щоб знайти людству новий шанс на життя.", 169, "Фантастика", "/source/matrix-poster.webp", 2014, "Інтерстеллар", "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov" },
                    { 16, "12+", new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "ComingSoon", "Команда професіоналів занурюється у сни, щоб вкрасти ідеї та посіяти нову думку в підсвідомості.", 148, "Трилер", "/source/images.jpeg", 2010, "Початок", "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov" },
                    { 17, "18+", new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ComingSoon", "Кілька історій із життя злочинного Лос-Анджелеса переплітаються у культовій кримінальній драмі.", 154, "Кримінал", "/source/it-poster.jpg", 1994, "Кримінальне чтиво", "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov" }
                });

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SessionTime",
                value: new DateTime(2026, 7, 3, 9, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 2,
                column: "SessionTime",
                value: new DateTime(2026, 7, 3, 12, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 3,
                column: "SessionTime",
                value: new DateTime(2026, 7, 3, 17, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 4,
                column: "SessionTime",
                value: new DateTime(2026, 7, 3, 19, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 5,
                column: "SessionTime",
                value: new DateTime(2026, 7, 4, 9, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 6,
                column: "SessionTime",
                value: new DateTime(2026, 7, 4, 12, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 7,
                column: "SessionTime",
                value: new DateTime(2026, 7, 4, 15, 55, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 8,
                column: "SessionTime",
                value: new DateTime(2026, 7, 4, 19, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 9,
                column: "SessionTime",
                value: new DateTime(2026, 7, 5, 9, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 10,
                column: "SessionTime",
                value: new DateTime(2026, 7, 5, 12, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 11,
                column: "SessionTime",
                value: new DateTime(2026, 7, 5, 15, 55, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 12,
                column: "SessionTime",
                value: new DateTime(2026, 7, 5, 17, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 13,
                column: "SessionTime",
                value: new DateTime(2026, 7, 6, 9, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 14,
                column: "SessionTime",
                value: new DateTime(2026, 7, 6, 15, 55, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 15,
                column: "SessionTime",
                value: new DateTime(2026, 7, 6, 17, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 16,
                column: "SessionTime",
                value: new DateTime(2026, 7, 7, 9, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 17,
                column: "SessionTime",
                value: new DateTime(2026, 7, 7, 12, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 18,
                column: "SessionTime",
                value: new DateTime(2026, 7, 7, 15, 55, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 19,
                column: "SessionTime",
                value: new DateTime(2026, 7, 7, 17, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 20,
                column: "SessionTime",
                value: new DateTime(2026, 7, 7, 19, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 21,
                column: "SessionTime",
                value: new DateTime(2026, 7, 8, 12, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 22,
                column: "SessionTime",
                value: new DateTime(2026, 7, 8, 15, 55, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 23,
                column: "SessionTime",
                value: new DateTime(2026, 7, 8, 19, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "FilmId", "HallId", "Price", "SessionTime" },
                values: new object[,]
                {
                    { 24, 1, 1, 180m, new DateTime(2026, 7, 6, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, 1, 2, 220m, new DateTime(2026, 7, 6, 14, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 26, 1, 2, 220m, new DateTime(2026, 7, 6, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, 1, 1, 180m, new DateTime(2026, 7, 9, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, 1, 2, 220m, new DateTime(2026, 7, 9, 14, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 29, 1, 2, 220m, new DateTime(2026, 7, 9, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, 1, 1, 180m, new DateTime(2026, 7, 12, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, 1, 2, 220m, new DateTime(2026, 7, 12, 14, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 32, 1, 2, 220m, new DateTime(2026, 7, 12, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, 1, 1, 180m, new DateTime(2026, 7, 15, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 34, 1, 2, 220m, new DateTime(2026, 7, 15, 14, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 35, 1, 2, 220m, new DateTime(2026, 7, 15, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 36, 1, 1, 180m, new DateTime(2026, 7, 18, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 37, 1, 2, 220m, new DateTime(2026, 7, 18, 14, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 38, 1, 2, 220m, new DateTime(2026, 7, 18, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 39, 1, 1, 180m, new DateTime(2026, 7, 21, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 40, 1, 2, 220m, new DateTime(2026, 7, 21, 14, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 41, 1, 2, 220m, new DateTime(2026, 7, 21, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 42, 1, 1, 180m, new DateTime(2026, 7, 24, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 43, 1, 2, 220m, new DateTime(2026, 7, 24, 14, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 44, 1, 2, 220m, new DateTime(2026, 7, 24, 19, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 45, 2, 1, 180m, new DateTime(2026, 7, 7, 10, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 46, 2, 2, 220m, new DateTime(2026, 7, 7, 15, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 47, 2, 3, 280m, new DateTime(2026, 7, 7, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 48, 2, 1, 180m, new DateTime(2026, 7, 10, 10, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 49, 2, 2, 220m, new DateTime(2026, 7, 10, 15, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 50, 2, 3, 280m, new DateTime(2026, 7, 10, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 51, 2, 1, 180m, new DateTime(2026, 7, 13, 10, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 52, 2, 2, 220m, new DateTime(2026, 7, 13, 15, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 53, 2, 3, 280m, new DateTime(2026, 7, 13, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 54, 2, 1, 180m, new DateTime(2026, 7, 16, 10, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 55, 2, 2, 220m, new DateTime(2026, 7, 16, 15, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 56, 2, 3, 280m, new DateTime(2026, 7, 16, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 57, 2, 1, 180m, new DateTime(2026, 7, 19, 10, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 58, 2, 2, 220m, new DateTime(2026, 7, 19, 15, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 59, 2, 3, 280m, new DateTime(2026, 7, 19, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 60, 2, 1, 180m, new DateTime(2026, 7, 22, 10, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 61, 2, 2, 220m, new DateTime(2026, 7, 22, 15, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 62, 2, 3, 280m, new DateTime(2026, 7, 22, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 63, 2, 1, 180m, new DateTime(2026, 7, 25, 10, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 64, 2, 2, 220m, new DateTime(2026, 7, 25, 15, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 65, 2, 3, 280m, new DateTime(2026, 7, 25, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 66, 3, 1, 160m, new DateTime(2026, 7, 8, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 67, 3, 2, 190m, new DateTime(2026, 7, 8, 16, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 68, 3, 2, 190m, new DateTime(2026, 7, 8, 20, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 69, 3, 1, 160m, new DateTime(2026, 7, 11, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 70, 3, 2, 190m, new DateTime(2026, 7, 11, 16, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 71, 3, 2, 190m, new DateTime(2026, 7, 11, 20, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 72, 3, 1, 160m, new DateTime(2026, 7, 14, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 73, 3, 2, 190m, new DateTime(2026, 7, 14, 16, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 74, 3, 2, 190m, new DateTime(2026, 7, 14, 20, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 75, 3, 1, 160m, new DateTime(2026, 7, 17, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 76, 3, 2, 190m, new DateTime(2026, 7, 17, 16, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 77, 3, 2, 190m, new DateTime(2026, 7, 17, 20, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 78, 3, 1, 160m, new DateTime(2026, 7, 20, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 79, 3, 2, 190m, new DateTime(2026, 7, 20, 16, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 80, 3, 2, 190m, new DateTime(2026, 7, 20, 20, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 81, 3, 1, 160m, new DateTime(2026, 7, 23, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 82, 3, 2, 190m, new DateTime(2026, 7, 23, 16, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 83, 3, 2, 190m, new DateTime(2026, 7, 23, 20, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 84, 3, 1, 160m, new DateTime(2026, 7, 26, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 85, 3, 2, 190m, new DateTime(2026, 7, 26, 16, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 86, 3, 2, 190m, new DateTime(2026, 7, 26, 20, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 87, 4, 1, 160m, new DateTime(2026, 7, 9, 10, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 88, 4, 2, 190m, new DateTime(2026, 7, 9, 15, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 89, 4, 3, 250m, new DateTime(2026, 7, 9, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 90, 4, 1, 160m, new DateTime(2026, 7, 12, 10, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 91, 4, 2, 190m, new DateTime(2026, 7, 12, 15, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 92, 4, 3, 250m, new DateTime(2026, 7, 12, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 93, 4, 1, 160m, new DateTime(2026, 7, 15, 10, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 94, 4, 2, 190m, new DateTime(2026, 7, 15, 15, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 95, 4, 3, 250m, new DateTime(2026, 7, 15, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 96, 4, 1, 160m, new DateTime(2026, 7, 18, 10, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 97, 4, 2, 190m, new DateTime(2026, 7, 18, 15, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 98, 4, 3, 250m, new DateTime(2026, 7, 18, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 99, 4, 1, 160m, new DateTime(2026, 7, 21, 10, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 100, 4, 2, 190m, new DateTime(2026, 7, 21, 15, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 101, 4, 3, 250m, new DateTime(2026, 7, 21, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 102, 4, 1, 160m, new DateTime(2026, 7, 24, 10, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 103, 4, 2, 190m, new DateTime(2026, 7, 24, 15, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 104, 4, 3, 250m, new DateTime(2026, 7, 24, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 105, 4, 1, 160m, new DateTime(2026, 7, 27, 10, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 106, 4, 2, 190m, new DateTime(2026, 7, 27, 15, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 107, 4, 3, 250m, new DateTime(2026, 7, 27, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 108, 5, 1, 170m, new DateTime(2026, 7, 10, 11, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 109, 5, 2, 200m, new DateTime(2026, 7, 10, 16, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 110, 5, 3, 260m, new DateTime(2026, 7, 10, 20, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 111, 5, 1, 170m, new DateTime(2026, 7, 13, 11, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 112, 5, 2, 200m, new DateTime(2026, 7, 13, 16, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 113, 5, 3, 260m, new DateTime(2026, 7, 13, 20, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 114, 5, 1, 170m, new DateTime(2026, 7, 16, 11, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 115, 5, 2, 200m, new DateTime(2026, 7, 16, 16, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 116, 5, 3, 260m, new DateTime(2026, 7, 16, 20, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 117, 5, 1, 170m, new DateTime(2026, 7, 19, 11, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 118, 5, 2, 200m, new DateTime(2026, 7, 19, 16, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 119, 5, 3, 260m, new DateTime(2026, 7, 19, 20, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 120, 5, 1, 170m, new DateTime(2026, 7, 22, 11, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 121, 5, 2, 200m, new DateTime(2026, 7, 22, 16, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 122, 5, 3, 260m, new DateTime(2026, 7, 22, 20, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 123, 5, 1, 170m, new DateTime(2026, 7, 25, 11, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 124, 5, 2, 200m, new DateTime(2026, 7, 25, 16, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 125, 5, 3, 260m, new DateTime(2026, 7, 25, 20, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 126, 5, 1, 170m, new DateTime(2026, 7, 28, 11, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 127, 5, 2, 200m, new DateTime(2026, 7, 28, 16, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 128, 5, 3, 260m, new DateTime(2026, 7, 28, 20, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 129, 6, 1, 180m, new DateTime(2026, 7, 11, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 130, 6, 2, 220m, new DateTime(2026, 7, 11, 15, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 131, 6, 3, 280m, new DateTime(2026, 7, 11, 20, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 132, 6, 1, 180m, new DateTime(2026, 7, 14, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 133, 6, 2, 220m, new DateTime(2026, 7, 14, 15, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 134, 6, 3, 280m, new DateTime(2026, 7, 14, 20, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 135, 6, 1, 180m, new DateTime(2026, 7, 17, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 136, 6, 2, 220m, new DateTime(2026, 7, 17, 15, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 137, 6, 3, 280m, new DateTime(2026, 7, 17, 20, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 138, 6, 1, 180m, new DateTime(2026, 7, 20, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 139, 6, 2, 220m, new DateTime(2026, 7, 20, 15, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 140, 6, 3, 280m, new DateTime(2026, 7, 20, 20, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 141, 6, 1, 180m, new DateTime(2026, 7, 23, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 142, 6, 2, 220m, new DateTime(2026, 7, 23, 15, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 143, 6, 3, 280m, new DateTime(2026, 7, 23, 20, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 144, 6, 1, 180m, new DateTime(2026, 7, 26, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 145, 6, 2, 220m, new DateTime(2026, 7, 26, 15, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 146, 6, 3, 280m, new DateTime(2026, 7, 26, 20, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 147, 6, 1, 180m, new DateTime(2026, 7, 29, 10, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 148, 6, 2, 220m, new DateTime(2026, 7, 29, 15, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 149, 6, 3, 280m, new DateTime(2026, 7, 29, 20, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 150, 9, 1, 170m, new DateTime(2026, 7, 3, 10, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 151, 9, 2, 210m, new DateTime(2026, 7, 3, 14, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 152, 9, 3, 260m, new DateTime(2026, 7, 3, 19, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 153, 9, 1, 170m, new DateTime(2026, 7, 6, 10, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 154, 9, 2, 210m, new DateTime(2026, 7, 6, 14, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 155, 9, 3, 260m, new DateTime(2026, 7, 6, 19, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 156, 9, 1, 170m, new DateTime(2026, 7, 9, 10, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 157, 9, 2, 210m, new DateTime(2026, 7, 9, 14, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 158, 9, 3, 260m, new DateTime(2026, 7, 9, 19, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 159, 9, 1, 170m, new DateTime(2026, 7, 12, 10, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 160, 9, 2, 210m, new DateTime(2026, 7, 12, 14, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 161, 9, 3, 260m, new DateTime(2026, 7, 12, 19, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 162, 9, 1, 170m, new DateTime(2026, 7, 15, 10, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 163, 9, 2, 210m, new DateTime(2026, 7, 15, 14, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 164, 9, 3, 260m, new DateTime(2026, 7, 15, 19, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 165, 9, 1, 170m, new DateTime(2026, 7, 18, 10, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 166, 9, 2, 210m, new DateTime(2026, 7, 18, 14, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 167, 9, 3, 260m, new DateTime(2026, 7, 18, 19, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 168, 9, 1, 170m, new DateTime(2026, 7, 21, 10, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 169, 9, 2, 210m, new DateTime(2026, 7, 21, 14, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 170, 9, 3, 260m, new DateTime(2026, 7, 21, 19, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 171, 9, 1, 170m, new DateTime(2026, 7, 24, 10, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 172, 9, 2, 210m, new DateTime(2026, 7, 24, 14, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 173, 9, 3, 260m, new DateTime(2026, 7, 24, 19, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 174, 10, 1, 190m, new DateTime(2026, 7, 4, 9, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 175, 10, 2, 230m, new DateTime(2026, 7, 4, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 176, 10, 3, 270m, new DateTime(2026, 7, 4, 19, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 177, 10, 1, 190m, new DateTime(2026, 7, 7, 9, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 178, 10, 2, 230m, new DateTime(2026, 7, 7, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 179, 10, 3, 270m, new DateTime(2026, 7, 7, 19, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 180, 10, 1, 190m, new DateTime(2026, 7, 10, 9, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 181, 10, 2, 230m, new DateTime(2026, 7, 10, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 182, 10, 3, 270m, new DateTime(2026, 7, 10, 19, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 183, 10, 1, 190m, new DateTime(2026, 7, 13, 9, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 184, 10, 2, 230m, new DateTime(2026, 7, 13, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 185, 10, 3, 270m, new DateTime(2026, 7, 13, 19, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 186, 10, 1, 190m, new DateTime(2026, 7, 16, 9, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 187, 10, 2, 230m, new DateTime(2026, 7, 16, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 188, 10, 3, 270m, new DateTime(2026, 7, 16, 19, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 189, 10, 1, 190m, new DateTime(2026, 7, 19, 9, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 190, 10, 2, 230m, new DateTime(2026, 7, 19, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 191, 10, 3, 270m, new DateTime(2026, 7, 19, 19, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 192, 10, 1, 190m, new DateTime(2026, 7, 22, 9, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 193, 10, 2, 230m, new DateTime(2026, 7, 22, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 194, 10, 3, 270m, new DateTime(2026, 7, 22, 19, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 195, 10, 1, 190m, new DateTime(2026, 7, 25, 9, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 196, 10, 2, 230m, new DateTime(2026, 7, 25, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 197, 10, 3, 270m, new DateTime(2026, 7, 25, 19, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 198, 11, 1, 160m, new DateTime(2026, 7, 5, 10, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 199, 11, 2, 200m, new DateTime(2026, 7, 5, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 200, 11, 2, 200m, new DateTime(2026, 7, 5, 18, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 201, 11, 1, 160m, new DateTime(2026, 7, 8, 10, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 202, 11, 2, 200m, new DateTime(2026, 7, 8, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 203, 11, 2, 200m, new DateTime(2026, 7, 8, 18, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 204, 11, 1, 160m, new DateTime(2026, 7, 11, 10, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 205, 11, 2, 200m, new DateTime(2026, 7, 11, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 206, 11, 2, 200m, new DateTime(2026, 7, 11, 18, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 207, 11, 1, 160m, new DateTime(2026, 7, 14, 10, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 208, 11, 2, 200m, new DateTime(2026, 7, 14, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 209, 11, 2, 200m, new DateTime(2026, 7, 14, 18, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 210, 11, 1, 160m, new DateTime(2026, 7, 17, 10, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 211, 11, 2, 200m, new DateTime(2026, 7, 17, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 212, 11, 2, 200m, new DateTime(2026, 7, 17, 18, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 213, 11, 1, 160m, new DateTime(2026, 7, 20, 10, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 214, 11, 2, 200m, new DateTime(2026, 7, 20, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 215, 11, 2, 200m, new DateTime(2026, 7, 20, 18, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 216, 11, 1, 160m, new DateTime(2026, 7, 23, 10, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 217, 11, 2, 200m, new DateTime(2026, 7, 23, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 218, 11, 2, 200m, new DateTime(2026, 7, 23, 18, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 219, 11, 1, 160m, new DateTime(2026, 7, 26, 10, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 220, 11, 2, 200m, new DateTime(2026, 7, 26, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 221, 11, 2, 200m, new DateTime(2026, 7, 26, 18, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 222, 12, 1, 150m, new DateTime(2026, 7, 6, 9, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 223, 12, 2, 180m, new DateTime(2026, 7, 6, 13, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 224, 12, 3, 220m, new DateTime(2026, 7, 6, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 225, 12, 1, 150m, new DateTime(2026, 7, 9, 9, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 226, 12, 2, 180m, new DateTime(2026, 7, 9, 13, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 227, 12, 3, 220m, new DateTime(2026, 7, 9, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 228, 12, 1, 150m, new DateTime(2026, 7, 12, 9, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 229, 12, 2, 180m, new DateTime(2026, 7, 12, 13, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 230, 12, 3, 220m, new DateTime(2026, 7, 12, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 231, 12, 1, 150m, new DateTime(2026, 7, 15, 9, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 232, 12, 2, 180m, new DateTime(2026, 7, 15, 13, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 233, 12, 3, 220m, new DateTime(2026, 7, 15, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 234, 12, 1, 150m, new DateTime(2026, 7, 18, 9, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 235, 12, 2, 180m, new DateTime(2026, 7, 18, 13, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 236, 12, 3, 220m, new DateTime(2026, 7, 18, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 237, 12, 1, 150m, new DateTime(2026, 7, 21, 9, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 238, 12, 2, 180m, new DateTime(2026, 7, 21, 13, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 239, 12, 3, 220m, new DateTime(2026, 7, 21, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 240, 12, 1, 150m, new DateTime(2026, 7, 24, 9, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 241, 12, 2, 180m, new DateTime(2026, 7, 24, 13, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 242, 12, 3, 220m, new DateTime(2026, 7, 24, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 243, 12, 1, 150m, new DateTime(2026, 7, 27, 9, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 244, 12, 2, 180m, new DateTime(2026, 7, 27, 13, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 245, 12, 3, 220m, new DateTime(2026, 7, 27, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 246, 13, 1, 210m, new DateTime(2026, 7, 7, 10, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 247, 13, 2, 250m, new DateTime(2026, 7, 7, 15, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 248, 13, 3, 300m, new DateTime(2026, 7, 7, 20, 25, 0, 0, DateTimeKind.Unspecified) },
                    { 249, 13, 1, 210m, new DateTime(2026, 7, 10, 10, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 250, 13, 2, 250m, new DateTime(2026, 7, 10, 15, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 251, 13, 3, 300m, new DateTime(2026, 7, 10, 20, 25, 0, 0, DateTimeKind.Unspecified) },
                    { 252, 13, 1, 210m, new DateTime(2026, 7, 13, 10, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 253, 13, 2, 250m, new DateTime(2026, 7, 13, 15, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 254, 13, 3, 300m, new DateTime(2026, 7, 13, 20, 25, 0, 0, DateTimeKind.Unspecified) },
                    { 255, 13, 1, 210m, new DateTime(2026, 7, 16, 10, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 256, 13, 2, 250m, new DateTime(2026, 7, 16, 15, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 257, 13, 3, 300m, new DateTime(2026, 7, 16, 20, 25, 0, 0, DateTimeKind.Unspecified) },
                    { 258, 13, 1, 210m, new DateTime(2026, 7, 19, 10, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 259, 13, 2, 250m, new DateTime(2026, 7, 19, 15, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 260, 13, 3, 300m, new DateTime(2026, 7, 19, 20, 25, 0, 0, DateTimeKind.Unspecified) },
                    { 261, 13, 1, 210m, new DateTime(2026, 7, 22, 10, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 262, 13, 2, 250m, new DateTime(2026, 7, 22, 15, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 263, 13, 3, 300m, new DateTime(2026, 7, 22, 20, 25, 0, 0, DateTimeKind.Unspecified) },
                    { 264, 13, 1, 210m, new DateTime(2026, 7, 25, 10, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 265, 13, 2, 250m, new DateTime(2026, 7, 25, 15, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 266, 13, 3, 300m, new DateTime(2026, 7, 25, 20, 25, 0, 0, DateTimeKind.Unspecified) },
                    { 267, 13, 1, 210m, new DateTime(2026, 7, 28, 10, 50, 0, 0, DateTimeKind.Unspecified) },
                    { 268, 13, 2, 250m, new DateTime(2026, 7, 28, 15, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 269, 13, 3, 300m, new DateTime(2026, 7, 28, 20, 25, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 260);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 265);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 266);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 267);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 268);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 1,
                column: "AvailabilityDate",
                value: new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 2,
                column: "AvailabilityDate",
                value: new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 3,
                column: "AvailabilityDate",
                value: new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AvailabilityDate", "Title" },
                values: new object[] { new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Matrix" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 5,
                column: "AvailabilityDate",
                value: new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AvailabilityDate", "Title" },
                values: new object[] { new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avengers" });

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SessionTime",
                value: new DateTime(2026, 5, 5, 9, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 2,
                column: "SessionTime",
                value: new DateTime(2026, 5, 5, 12, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 3,
                column: "SessionTime",
                value: new DateTime(2026, 5, 5, 17, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 4,
                column: "SessionTime",
                value: new DateTime(2026, 5, 5, 19, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 5,
                column: "SessionTime",
                value: new DateTime(2026, 5, 6, 9, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 6,
                column: "SessionTime",
                value: new DateTime(2026, 5, 6, 12, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 7,
                column: "SessionTime",
                value: new DateTime(2026, 5, 6, 15, 55, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 8,
                column: "SessionTime",
                value: new DateTime(2026, 5, 6, 19, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 9,
                column: "SessionTime",
                value: new DateTime(2026, 5, 7, 9, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 10,
                column: "SessionTime",
                value: new DateTime(2026, 5, 7, 12, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 11,
                column: "SessionTime",
                value: new DateTime(2026, 5, 7, 15, 55, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 12,
                column: "SessionTime",
                value: new DateTime(2026, 5, 7, 17, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 13,
                column: "SessionTime",
                value: new DateTime(2026, 5, 8, 9, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 14,
                column: "SessionTime",
                value: new DateTime(2026, 5, 8, 15, 55, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 15,
                column: "SessionTime",
                value: new DateTime(2026, 5, 8, 17, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 16,
                column: "SessionTime",
                value: new DateTime(2026, 5, 9, 9, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 17,
                column: "SessionTime",
                value: new DateTime(2026, 5, 9, 12, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 18,
                column: "SessionTime",
                value: new DateTime(2026, 5, 9, 15, 55, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 19,
                column: "SessionTime",
                value: new DateTime(2026, 5, 9, 17, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 20,
                column: "SessionTime",
                value: new DateTime(2026, 5, 9, 19, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 21,
                column: "SessionTime",
                value: new DateTime(2026, 5, 10, 12, 45, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 22,
                column: "SessionTime",
                value: new DateTime(2026, 5, 10, 15, 55, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 23,
                column: "SessionTime",
                value: new DateTime(2026, 5, 10, 19, 45, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
