using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaPlus.CinemaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class RestoreOriginalCatalogueSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "DurationMinutes", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "Нова подорож Пандорою, де родина Саллі стикається з вогняним народом та небезпекою, що змінює майбутнє На'ві.", 190, "/source/avatar-poster.webp", 2025, "Аватар: Вогонь і попіл" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AgeRestriction", "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "12+", "Боротьба за Арракіс виходить за межі однієї планети, а Пол Атрейдес робить вибір, що змінить майбутнє Імперіуму.", 160, "Фантастика", "/source/images.jpeg", 2026, "Дюна: Частина третя" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "Друзі з маленького міста зустрічаються з давнім страхом, який повертається у найтемніших образах.", 135, "Жахи", "/source/it-poster.jpg", 2017, "Воно" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AgeRestriction", "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "16+", "Нео відкриває правду про світ, у якому живе, та вступає у боротьбу з системою Матриці.", 136, "Фантастика", "/source/matrix-poster.webp", 1999, "Matrix" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AgeRestriction", "AvailabilityDate", "AvailabilityStatus", "Description", "DurationMinutes", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "16+", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "NowShowing", "Остання битва друзів з Гокінса проти темряви, що загрожує їхньому місту та всьому світу.", 120, "/source/str-things-poster.jpg", 2025, "Дивні дива: Фінал" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AgeRestriction", "AvailabilityDate", "AvailabilityStatus", "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "12+", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "NowShowing", "Команда супергероїв об'єднується, щоб зупинити загрозу, з якою неможливо впоратися поодинці.", 143, "Екшн", "/source/avengers-poster.jpg", 2012, "Avengers" });

            migrationBuilder.InsertData(
                table: "Films",
                columns: new[] { "Id", "AgeRestriction", "AvailabilityDate", "AvailabilityStatus", "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title" },
                values: new object[,]
                {
                    { 7, "12+", new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ComingSoon", "Екіпаж вирушає до межі Сонячної системи, де на них чекає відкриття, здатне змінити історію людства.", 118, "Пригоди", "/source/hero-img.jpg", 2026, "Місія: Сонячний рубіж" },
                    { 8, "0+", new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "ComingSoon", "Сімейна пригода про магічний ліс, дружбу та силу сміливого вибору.", 104, "Анімація", "/source/form-hero.png", 2026, "Лісова легенда" }
                });

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
                columns: new[] { "HallId", "Price", "SessionTime" },
                values: new object[] { 1, 180m, new DateTime(2026, 5, 5, 12, 45, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FilmId", "HallId", "Price", "SessionTime" },
                values: new object[] { 1, 2, 220m, new DateTime(2026, 5, 5, 17, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FilmId", "HallId", "Price", "SessionTime" },
                values: new object[] { 1, 2, 220m, new DateTime(2026, 5, 5, 19, 45, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FilmId", "HallId", "Price", "SessionTime" },
                values: new object[] { 2, 1, 180m, new DateTime(2026, 5, 6, 9, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "FilmId", "HallId", "Price", "SessionTime" },
                values: new object[,]
                {
                    { 6, 2, 1, 180m, new DateTime(2026, 5, 6, 12, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 2, 2, 220m, new DateTime(2026, 5, 6, 15, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 2, 2, 220m, new DateTime(2026, 5, 6, 19, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 3, 1, 160m, new DateTime(2026, 5, 7, 9, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 3, 1, 160m, new DateTime(2026, 5, 7, 12, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 3, 2, 190m, new DateTime(2026, 5, 7, 15, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 3, 2, 190m, new DateTime(2026, 5, 7, 17, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 4, 1, 160m, new DateTime(2026, 5, 8, 9, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 4, 2, 190m, new DateTime(2026, 5, 8, 15, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 4, 2, 190m, new DateTime(2026, 5, 8, 17, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 5, 1, 170m, new DateTime(2026, 5, 9, 9, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 5, 1, 170m, new DateTime(2026, 5, 9, 12, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 5, 2, 200m, new DateTime(2026, 5, 9, 15, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 5, 2, 200m, new DateTime(2026, 5, 9, 17, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 5, 3, 260m, new DateTime(2026, 5, 9, 19, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 6, 1, 180m, new DateTime(2026, 5, 10, 12, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 6, 2, 220m, new DateTime(2026, 5, 10, 15, 55, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 6, 3, 280m, new DateTime(2026, 5, 10, 19, 45, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "DurationMinutes", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "Пол Атрід продовжує подорож пустелею Арракіса та об'єднується з фременами.", 166, "/images/posters/dune-2.jpg", 2024, "Дюна: Частина друга" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AgeRestriction", "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "0+", "Нові емоції з'являються у житті Райлі та змінюють звичний порядок.", 96, "Анімація", "/images/posters/inside-out-2.jpg", 2024, "Думками навиворіт 2" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "Історія науковця, рішення якого змінили хід світової історії.", 180, "Драма", "/images/posters/oppenheimer.jpg", 2023, "Оппенгеймер" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AgeRestriction", "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "0+", "Українська анімаційна історія про магію лісу, вибір і любов.", 99, "Анімація", "/images/posters/mavka.jpg", 2023, "Мавка. Лісова пісня" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AgeRestriction", "AvailabilityDate", "AvailabilityStatus", "Description", "DurationMinutes", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "12+", new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ComingSoon", "Нова подорож Пандорою та знайомство з іншим народом На'ві.", 170, "/images/posters/avatar-3.jpg", 2026, "Аватар 3" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AgeRestriction", "AvailabilityDate", "AvailabilityStatus", "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title" },
                values: new object[] { "0+", new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "ComingSoon", "Повернення улюблених героїв у новій пригоді для всієї родини.", 100, "Анімація", "/images/posters/shrek-5.jpg", 2026, "Шрек 5" });

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SessionTime",
                value: new DateTime(2026, 5, 5, 18, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HallId", "Price", "SessionTime" },
                values: new object[] { 2, 240m, new DateTime(2026, 5, 6, 20, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FilmId", "HallId", "Price", "SessionTime" },
                values: new object[] { 2, 1, 160m, new DateTime(2026, 5, 7, 16, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FilmId", "HallId", "Price", "SessionTime" },
                values: new object[] { 3, 3, 320m, new DateTime(2026, 6, 2, 19, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Sessions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FilmId", "HallId", "Price", "SessionTime" },
                values: new object[] { 4, 2, 170m, new DateTime(2026, 6, 12, 14, 30, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
