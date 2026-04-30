using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaPlus.CinemaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Genre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    AgeRestriction = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    PosterPath = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    AvailabilityDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AvailabilityStatus = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Halls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    Technology = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    RowsCount = table.Column<int>(type: "int", nullable: false),
                    SeatsPerRow = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halls", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "varchar(180)", maxLength: 180, nullable: false),
                    Phone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    HallId = table.Column<int>(type: "int", nullable: false),
                    RowNumber = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FilmId = table.Column<int>(type: "int", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: false),
                    SessionTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sessions_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    TicketCode = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    CustomerName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    CustomerEmail = table.Column<string>(type: "varchar(180)", maxLength: 180, nullable: false),
                    CustomerPhone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BookedSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookedSeats_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookedSeats_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookedSeats_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NotificationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "varchar(180)", maxLength: 180, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationLogs_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Films",
                columns: new[] { "Id", "AgeRestriction", "AvailabilityDate", "AvailabilityStatus", "Description", "DurationMinutes", "Genre", "PosterPath", "ReleaseYear", "Title" },
                values: new object[,]
                {
                    { 1, "12+", new DateOnly(2026, 5, 1), "NowShowing", "Пол Атрід продовжує подорож пустелею Арракіса та об'єднується з фременами.", 166, "Фантастика", "/images/posters/dune-2.jpg", 2024, "Дюна: Частина друга" },
                    { 2, "0+", new DateOnly(2026, 5, 3), "NowShowing", "Нові емоції з'являються у житті Райлі та змінюють звичний порядок.", 96, "Анімація", "/images/posters/inside-out-2.jpg", 2024, "Думками навиворіт 2" },
                    { 3, "16+", new DateOnly(2026, 6, 1), "NowShowing", "Історія науковця, рішення якого змінили хід світової історії.", 180, "Драма", "/images/posters/oppenheimer.jpg", 2023, "Оппенгеймер" },
                    { 4, "0+", new DateOnly(2026, 6, 10), "NowShowing", "Українська анімаційна історія про магію лісу, вибір і любов.", 99, "Анімація", "/images/posters/mavka.jpg", 2023, "Мавка. Лісова пісня" },
                    { 5, "12+", new DateOnly(2026, 9, 15), "ComingSoon", "Нова подорож Пандорою та знайомство з іншим народом На'ві.", 170, "Фантастика", "/images/posters/avatar-3.jpg", 2026, "Аватар 3" },
                    { 6, "0+", new DateOnly(2026, 10, 20), "ComingSoon", "Повернення улюблених героїв у новій пригоді для всієї родини.", 100, "Анімація", "/images/posters/shrek-5.jpg", 2026, "Шрек 5" }
                });

            migrationBuilder.InsertData(
                table: "Halls",
                columns: new[] { "Id", "Name", "RowsCount", "SeatsPerRow", "Technology" },
                values: new object[,]
                {
                    { 1, "Зал 1", 6, 10, "2D" },
                    { 2, "Зал 2", 8, 12, "3D" },
                    { 3, "VIP зал", 5, 8, "IMAX" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "PasswordHash", "Phone", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 29, 12, 0, 0, 0, DateTimeKind.Unspecified), "admin@cinemaplus.local", "Адміністратор CinemaPlus", "DEMO_PASSWORD_HASH_admin123", "+380501112233", "Admin" },
                    { 2, new DateTime(2026, 4, 29, 12, 0, 0, 0, DateTimeKind.Unspecified), "client@cinemaplus.local", "Олена Коваль", "DEMO_PASSWORD_HASH_client123", "+380672223344", "Client" }
                });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "HallId", "RowNumber", "SeatNumber" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 1, 1, 2 },
                    { 3, 1, 1, 3 },
                    { 4, 1, 1, 4 },
                    { 5, 1, 1, 5 },
                    { 6, 1, 1, 6 },
                    { 7, 1, 1, 7 },
                    { 8, 1, 1, 8 },
                    { 9, 1, 1, 9 },
                    { 10, 1, 1, 10 },
                    { 11, 1, 2, 1 },
                    { 12, 1, 2, 2 },
                    { 13, 1, 2, 3 },
                    { 14, 1, 2, 4 },
                    { 15, 1, 2, 5 },
                    { 16, 1, 2, 6 },
                    { 17, 1, 2, 7 },
                    { 18, 1, 2, 8 },
                    { 19, 1, 2, 9 },
                    { 20, 1, 2, 10 },
                    { 21, 1, 3, 1 },
                    { 22, 1, 3, 2 },
                    { 23, 1, 3, 3 },
                    { 24, 1, 3, 4 },
                    { 25, 1, 3, 5 },
                    { 26, 1, 3, 6 },
                    { 27, 1, 3, 7 },
                    { 28, 1, 3, 8 },
                    { 29, 1, 3, 9 },
                    { 30, 1, 3, 10 },
                    { 31, 1, 4, 1 },
                    { 32, 1, 4, 2 },
                    { 33, 1, 4, 3 },
                    { 34, 1, 4, 4 },
                    { 35, 1, 4, 5 },
                    { 36, 1, 4, 6 },
                    { 37, 1, 4, 7 },
                    { 38, 1, 4, 8 },
                    { 39, 1, 4, 9 },
                    { 40, 1, 4, 10 },
                    { 41, 1, 5, 1 },
                    { 42, 1, 5, 2 },
                    { 43, 1, 5, 3 },
                    { 44, 1, 5, 4 },
                    { 45, 1, 5, 5 },
                    { 46, 1, 5, 6 },
                    { 47, 1, 5, 7 },
                    { 48, 1, 5, 8 },
                    { 49, 1, 5, 9 },
                    { 50, 1, 5, 10 },
                    { 51, 1, 6, 1 },
                    { 52, 1, 6, 2 },
                    { 53, 1, 6, 3 },
                    { 54, 1, 6, 4 },
                    { 55, 1, 6, 5 },
                    { 56, 1, 6, 6 },
                    { 57, 1, 6, 7 },
                    { 58, 1, 6, 8 },
                    { 59, 1, 6, 9 },
                    { 60, 1, 6, 10 },
                    { 61, 2, 1, 1 },
                    { 62, 2, 1, 2 },
                    { 63, 2, 1, 3 },
                    { 64, 2, 1, 4 },
                    { 65, 2, 1, 5 },
                    { 66, 2, 1, 6 },
                    { 67, 2, 1, 7 },
                    { 68, 2, 1, 8 },
                    { 69, 2, 1, 9 },
                    { 70, 2, 1, 10 },
                    { 71, 2, 1, 11 },
                    { 72, 2, 1, 12 },
                    { 73, 2, 2, 1 },
                    { 74, 2, 2, 2 },
                    { 75, 2, 2, 3 },
                    { 76, 2, 2, 4 },
                    { 77, 2, 2, 5 },
                    { 78, 2, 2, 6 },
                    { 79, 2, 2, 7 },
                    { 80, 2, 2, 8 },
                    { 81, 2, 2, 9 },
                    { 82, 2, 2, 10 },
                    { 83, 2, 2, 11 },
                    { 84, 2, 2, 12 },
                    { 85, 2, 3, 1 },
                    { 86, 2, 3, 2 },
                    { 87, 2, 3, 3 },
                    { 88, 2, 3, 4 },
                    { 89, 2, 3, 5 },
                    { 90, 2, 3, 6 },
                    { 91, 2, 3, 7 },
                    { 92, 2, 3, 8 },
                    { 93, 2, 3, 9 },
                    { 94, 2, 3, 10 },
                    { 95, 2, 3, 11 },
                    { 96, 2, 3, 12 },
                    { 97, 2, 4, 1 },
                    { 98, 2, 4, 2 },
                    { 99, 2, 4, 3 },
                    { 100, 2, 4, 4 },
                    { 101, 2, 4, 5 },
                    { 102, 2, 4, 6 },
                    { 103, 2, 4, 7 },
                    { 104, 2, 4, 8 },
                    { 105, 2, 4, 9 },
                    { 106, 2, 4, 10 },
                    { 107, 2, 4, 11 },
                    { 108, 2, 4, 12 },
                    { 109, 2, 5, 1 },
                    { 110, 2, 5, 2 },
                    { 111, 2, 5, 3 },
                    { 112, 2, 5, 4 },
                    { 113, 2, 5, 5 },
                    { 114, 2, 5, 6 },
                    { 115, 2, 5, 7 },
                    { 116, 2, 5, 8 },
                    { 117, 2, 5, 9 },
                    { 118, 2, 5, 10 },
                    { 119, 2, 5, 11 },
                    { 120, 2, 5, 12 },
                    { 121, 2, 6, 1 },
                    { 122, 2, 6, 2 },
                    { 123, 2, 6, 3 },
                    { 124, 2, 6, 4 },
                    { 125, 2, 6, 5 },
                    { 126, 2, 6, 6 },
                    { 127, 2, 6, 7 },
                    { 128, 2, 6, 8 },
                    { 129, 2, 6, 9 },
                    { 130, 2, 6, 10 },
                    { 131, 2, 6, 11 },
                    { 132, 2, 6, 12 },
                    { 133, 2, 7, 1 },
                    { 134, 2, 7, 2 },
                    { 135, 2, 7, 3 },
                    { 136, 2, 7, 4 },
                    { 137, 2, 7, 5 },
                    { 138, 2, 7, 6 },
                    { 139, 2, 7, 7 },
                    { 140, 2, 7, 8 },
                    { 141, 2, 7, 9 },
                    { 142, 2, 7, 10 },
                    { 143, 2, 7, 11 },
                    { 144, 2, 7, 12 },
                    { 145, 2, 8, 1 },
                    { 146, 2, 8, 2 },
                    { 147, 2, 8, 3 },
                    { 148, 2, 8, 4 },
                    { 149, 2, 8, 5 },
                    { 150, 2, 8, 6 },
                    { 151, 2, 8, 7 },
                    { 152, 2, 8, 8 },
                    { 153, 2, 8, 9 },
                    { 154, 2, 8, 10 },
                    { 155, 2, 8, 11 },
                    { 156, 2, 8, 12 },
                    { 157, 3, 1, 1 },
                    { 158, 3, 1, 2 },
                    { 159, 3, 1, 3 },
                    { 160, 3, 1, 4 },
                    { 161, 3, 1, 5 },
                    { 162, 3, 1, 6 },
                    { 163, 3, 1, 7 },
                    { 164, 3, 1, 8 },
                    { 165, 3, 2, 1 },
                    { 166, 3, 2, 2 },
                    { 167, 3, 2, 3 },
                    { 168, 3, 2, 4 },
                    { 169, 3, 2, 5 },
                    { 170, 3, 2, 6 },
                    { 171, 3, 2, 7 },
                    { 172, 3, 2, 8 },
                    { 173, 3, 3, 1 },
                    { 174, 3, 3, 2 },
                    { 175, 3, 3, 3 },
                    { 176, 3, 3, 4 },
                    { 177, 3, 3, 5 },
                    { 178, 3, 3, 6 },
                    { 179, 3, 3, 7 },
                    { 180, 3, 3, 8 },
                    { 181, 3, 4, 1 },
                    { 182, 3, 4, 2 },
                    { 183, 3, 4, 3 },
                    { 184, 3, 4, 4 },
                    { 185, 3, 4, 5 },
                    { 186, 3, 4, 6 },
                    { 187, 3, 4, 7 },
                    { 188, 3, 4, 8 },
                    { 189, 3, 5, 1 },
                    { 190, 3, 5, 2 },
                    { 191, 3, 5, 3 },
                    { 192, 3, 5, 4 },
                    { 193, 3, 5, 5 },
                    { 194, 3, 5, 6 },
                    { 195, 3, 5, 7 },
                    { 196, 3, 5, 8 }
                });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "FilmId", "HallId", "Price", "SessionTime" },
                values: new object[,]
                {
                    { 1, 1, 1, 180m, new DateTime(2026, 5, 5, 18, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, 2, 240m, new DateTime(2026, 5, 6, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 2, 1, 160m, new DateTime(2026, 5, 7, 16, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 3, 3, 320m, new DateTime(2026, 6, 2, 19, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 4, 2, 170m, new DateTime(2026, 6, 12, 14, 30, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingDate", "CustomerEmail", "CustomerName", "CustomerPhone", "SessionId", "Status", "TicketCode", "TotalPrice", "UserId" },
                values: new object[] { 1, new DateTime(2026, 5, 1, 10, 15, 0, 0, DateTimeKind.Unspecified), "client@cinemaplus.local", "Олена Коваль", "+380672223344", 1, "Paid", "CP-20260501-0001", 360m, 2 });

            migrationBuilder.InsertData(
                table: "BookedSeats",
                columns: new[] { "Id", "BookingId", "SeatId", "SessionId" },
                values: new object[,]
                {
                    { 1, 1, 25, 1 },
                    { 2, 1, 26, 1 }
                });

            migrationBuilder.InsertData(
                table: "NotificationLogs",
                columns: new[] { "Id", "BookingId", "CreatedDate", "Email", "Message", "Status" },
                values: new object[] { 1, 1, new DateTime(2026, 5, 1, 10, 16, 0, 0, DateTimeKind.Unspecified), "client@cinemaplus.local", "Підтвердження бронювання CP-20260501-0001", "Emulated" });

            migrationBuilder.CreateIndex(
                name: "IX_BookedSeats_BookingId_SeatId",
                table: "BookedSeats",
                columns: new[] { "BookingId", "SeatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookedSeats_SeatId",
                table: "BookedSeats",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_BookedSeats_SessionId_SeatId",
                table: "BookedSeats",
                columns: new[] { "SessionId", "SeatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingDate",
                table: "Bookings",
                column: "BookingDate");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SessionId",
                table: "Bookings",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TicketCode",
                table: "Bookings",
                column: "TicketCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Films_AvailabilityStatus_AvailabilityDate",
                table: "Films",
                columns: new[] { "AvailabilityStatus", "AvailabilityDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Films_Title",
                table: "Films",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Halls_Name",
                table: "Halls",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLogs_BookingId",
                table: "NotificationLogs",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_HallId_RowNumber_SeatNumber",
                table: "Seats",
                columns: new[] { "HallId", "RowNumber", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_FilmId_HallId_SessionTime",
                table: "Sessions",
                columns: new[] { "FilmId", "HallId", "SessionTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_HallId",
                table: "Sessions",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SessionTime",
                table: "Sessions",
                column: "SessionTime");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookedSeats");

            migrationBuilder.DropTable(
                name: "NotificationLogs");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Films");

            migrationBuilder.DropTable(
                name: "Halls");
        }
    }
}
