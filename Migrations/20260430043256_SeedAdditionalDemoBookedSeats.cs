using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaPlus.CinemaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdditionalDemoBookedSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingDate", "CustomerEmail", "CustomerName", "CustomerPhone", "SessionId", "Status", "TicketCode", "TotalPrice", "UserId" },
                values: new object[,]
                {
                    { 2, new DateTime(2026, 5, 2, 14, 5, 0, 0, DateTimeKind.Unspecified), "client@cinemaplus.local", "Олена Коваль", "+380672223344", 5, "Paid", "CP-20260502-0002", 360m, 2 },
                    { 3, new DateTime(2026, 5, 3, 18, 20, 0, 0, DateTimeKind.Unspecified), "marina.demo@cinemaplus.local", "Марина Стеценко", "+380931110022", 7, "Paid", "CP-20260503-0003", 660m, null },
                    { 4, new DateTime(2026, 5, 4, 11, 40, 0, 0, DateTimeKind.Unspecified), "igor.demo@cinemaplus.local", "Ігор Мельник", "+380661230045", 20, "Paid", "CP-20260504-0004", 520m, null }
                });

            migrationBuilder.InsertData(
                table: "BookedSeats",
                columns: new[] { "Id", "BookingId", "SeatId", "SessionId" },
                values: new object[,]
                {
                    { 3, 2, 14, 5 },
                    { 4, 2, 15, 5 },
                    { 5, 3, 73, 7 },
                    { 6, 3, 74, 7 },
                    { 7, 3, 75, 7 },
                    { 8, 4, 173, 20 },
                    { 9, 4, 174, 20 }
                });

            migrationBuilder.InsertData(
                table: "NotificationLogs",
                columns: new[] { "Id", "BookingId", "CreatedDate", "Email", "Message", "Status" },
                values: new object[,]
                {
                    { 2, 2, new DateTime(2026, 5, 2, 14, 6, 0, 0, DateTimeKind.Unspecified), "client@cinemaplus.local", "Підтвердження бронювання CP-20260502-0002", "Emulated" },
                    { 3, 3, new DateTime(2026, 5, 3, 18, 21, 0, 0, DateTimeKind.Unspecified), "marina.demo@cinemaplus.local", "Підтвердження бронювання CP-20260503-0003", "Emulated" },
                    { 4, 4, new DateTime(2026, 5, 4, 11, 41, 0, 0, DateTimeKind.Unspecified), "igor.demo@cinemaplus.local", "Підтвердження бронювання CP-20260504-0004", "Emulated" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "BookedSeats",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "NotificationLogs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NotificationLogs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NotificationLogs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
