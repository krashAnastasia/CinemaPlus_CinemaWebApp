using System.Net;
using System.Security.Cryptography;
using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Models;
using CinemaPlus.CinemaWebApp.Services;
using CinemaPlus.CinemaWebApp.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Tests.Integration;

public class TicketPdfAccessTests
{
    [Fact]
    public async Task TicketPdfEndpoint_RestrictsForeignUsers_AndAllowsAdminsAndGuests()
    {
        await using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        int ownerBookingId;
        int guestBookingId;

        await using (var db = factory.CreateDbContext())
        {
            var passwordService = new Pbkdf2PasswordService();
            db.Users.Add(new User
            {
                FullName = "Сторонній Користувач",
                Email = "outsider@test.local",
                Phone = "+380661112233",
                PasswordHash = passwordService.HashPassword("outsider123"),
                Role = "Client",
                CreatedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();

            var hall = new Hall
            {
                Name = $"PDF зал {Guid.NewGuid():N}",
                Technology = "2D",
                RowsCount = 1,
                SeatsPerRow = 3
            };

            var film = new Film
            {
                Title = $"PDF фільм {Guid.NewGuid():N}",
                Genre = "Драма",
                DurationMinutes = 100,
                Description = "Фільм для тесту PDF доступу.",
                ReleaseYear = 2026,
                AgeRestriction = "12+",
                PosterPath = "/source/hero-img.jpg",
                AvailabilityDate = new DateOnly(2026, 5, 1),
                AvailabilityStatus = "NowShowing"
            };

            db.Halls.Add(hall);
            db.Films.Add(film);
            await db.SaveChangesAsync();

            db.Seats.AddRange(
                new Seat { HallId = hall.Id, RowNumber = 1, SeatNumber = 1 },
                new Seat { HallId = hall.Id, RowNumber = 1, SeatNumber = 2 },
                new Seat { HallId = hall.Id, RowNumber = 1, SeatNumber = 3 });
            await db.SaveChangesAsync();

            var session = new Session
            {
                FilmId = film.Id,
                HallId = hall.Id,
                SessionTime = new DateTime(2026, 5, 15, 19, 0, 0),
                Price = 210m
            };

            db.Sessions.Add(session);
            await db.SaveChangesAsync();

            var seatIds = await db.Seats
                .Where(item => item.HallId == hall.Id)
                .OrderBy(item => item.SeatNumber)
                .Select(item => item.Id)
                .ToListAsync();

            var ownerBooking = new Booking
            {
                UserId = 2,
                SessionId = session.Id,
                BookingDate = DateTime.UtcNow,
                Status = "Paid",
                TotalPrice = 210m,
                TicketCode = "CP-OWNER-PDF",
                CustomerName = "Олена Коваль",
                CustomerEmail = "client@cinemaplus.local",
                CustomerPhone = "+380672223344"
            };

            var guestBooking = new Booking
            {
                UserId = null,
                SessionId = session.Id,
                BookingDate = DateTime.UtcNow,
                Status = "Paid",
                TotalPrice = 210m,
                TicketCode = "CP-GUEST-PDF",
                CustomerName = "Гість PDF",
                CustomerEmail = "guest.pdf@test.local",
                CustomerPhone = "+380501010101"
            };

            db.Bookings.AddRange(ownerBooking, guestBooking);
            await db.SaveChangesAsync();
            ownerBookingId = ownerBooking.Id;
            guestBookingId = guestBooking.Id;

            db.BookedSeats.AddRange(
                new BookedSeat { BookingId = ownerBookingId, SessionId = session.Id, SeatId = seatIds[0] },
                new BookedSeat { BookingId = guestBookingId, SessionId = session.Id, SeatId = seatIds[1] });
            await db.SaveChangesAsync();
        }

        await LoginAsync(client, "outsider@test.local", "outsider123");
        var foreignPdfResponse = await client.GetAsync($"/tickets/{ownerBookingId}/pdf");
        Assert.Equal(HttpStatusCode.Redirect, foreignPdfResponse.StatusCode);
        Assert.Contains("/account/access-denied", foreignPdfResponse.Headers.Location?.OriginalString);

        await client.GetAsync("/account/logout");
        await LoginAsync(client, "admin@cinemaplus.local", "admin123");
        var adminPdfResponse = await client.GetAsync($"/tickets/{ownerBookingId}/pdf");
        Assert.Equal(HttpStatusCode.OK, adminPdfResponse.StatusCode);
        Assert.Equal("application/pdf", adminPdfResponse.Content.Headers.ContentType?.MediaType);

        await client.GetAsync("/account/logout");
        var guestPdfResponse = await client.GetAsync($"/tickets/{guestBookingId}/pdf?ticketCode=CP-GUEST-PDF");
        Assert.Equal(HttpStatusCode.OK, guestPdfResponse.StatusCode);
        Assert.Equal("application/pdf", guestPdfResponse.Content.Headers.ContentType?.MediaType);
    }

    private static async Task LoginAsync(HttpClient client, string email, string password)
    {
        var loginPage = await client.GetStringAsync("/account/login");
        var token = TestHtml.ExtractAntiForgeryToken(loginPage);
        var response = await client.PostAsync(
            "/account/login",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = token,
                ["Email"] = email,
                ["Password"] = password
            }));

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }
}
