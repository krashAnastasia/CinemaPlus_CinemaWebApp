using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaPlus.CinemaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreDemoBookedSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingDate", "CustomerEmail", "CustomerName", "CustomerPhone", "SessionId", "Status", "TicketCode", "TotalPrice", "UserId" },
                values: new object[,]
                {
                    { 5, new DateTime(2026, 5, 5, 12, 5, 0, 0, DateTimeKind.Unspecified), "nataliia.demo@cinemaplus.local", "Наталія Гринь", "+380971230056", 3, "Paid", "CP-20260505-0005", 880m, null },
                    { 6, new DateTime(2026, 5, 5, 16, 35, 0, 0, DateTimeKind.Unspecified), "client@cinemaplus.local", "Олена Коваль", "+380672223344", 8, "Paid", "CP-20260505-0006", 660m, 2 },
                    { 7, new DateTime(2026, 5, 6, 9, 10, 0, 0, DateTimeKind.Unspecified), "andrii.demo@cinemaplus.local", "Андрій Савчук", "+380501450067", 16, "Paid", "CP-20260506-0007", 510m, null },
                    { 8, new DateTime(2026, 5, 6, 19, 5, 0, 0, DateTimeKind.Unspecified), "yuliia.demo@cinemaplus.local", "Юлія Бондар", "+380631220078", 23, "Paid", "CP-20260506-0008", 840m, null }
                });

            migrationBuilder.InsertData(
                table: "BookedSeats",
                columns: new[] { "Id", "BookingId", "SeatId", "SessionId" },
                values: new object[,]
                {
                    { 10, 5, 86, 3 },
                    { 11, 5, 87, 3 },
                    { 12, 5, 98, 3 },
                    { 13, 5, 99, 3 },
                    { 14, 6, 109, 8 },
                    { 15, 6, 110, 8 },
                    { 16, 6, 121, 8 },
                    { 17, 7, 31, 16 },
                    { 18, 7, 32, 16 },
                    { 19, 7, 33, 16 },
                    { 20, 8, 189, 23 },
                    { 21, 8, 190, 23 },
                    { 22, 8, 195, 23 }
                });

            migrationBuilder.InsertData(
                table: "NotificationLogs",
                columns: new[] { "Id", "BookingId", "CreatedDate", "Email", "Message", "Status" },
                values: new object[,]
                {
                    { 5, 5, new DateTime(2026, 5, 5, 12, 6, 0, 0, DateTimeKind.Unspecified), "nataliia.demo@cinemaplus.local", "Підтвердження бронювання CP-20260505-0005", "Emulated" },
                    { 6, 6, new DateTime(2026, 5, 5, 16, 36, 0, 0, DateTimeKind.Unspecified), "client@cinemaplus.local", "Підтвердження бронювання CP-20260505-0006", "Emulated" },
                    { 7, 7, new DateTime(2026, 5, 6, 9, 11, 0, 0, DateTimeKind.Unspecified), "andrii.demo@cinemaplus.local", "Підтвердження бронювання CP-20260506-0007", "Emulated" },
                    { 8, 8, new DateTime(2026, 5, 6, 19, 6, 0, 0, DateTimeKind.Unspecified), "yuliia.demo@cinemaplus.local", "Підтвердження бронювання CP-20260506-0008", "Emulated" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "NotificationLogs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "NotificationLogs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "NotificationLogs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "NotificationLogs",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
