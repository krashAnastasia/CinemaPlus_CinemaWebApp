using System.Net;
using System.Net.Http.Headers;
using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Models;
using CinemaPlus.CinemaWebApp.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Tests.Integration;

public class AdminFlowTests
{
    [Fact]
    public async Task AdminCrudAndReportsFlow_Works()
    {
        await using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var guestAdminResponse = await client.GetAsync("/admin/films");
        Assert.Equal(HttpStatusCode.Redirect, guestAdminResponse.StatusCode);
        Assert.Contains("/account/login", guestAdminResponse.Headers.Location?.OriginalString);

        await LoginAsync(client, "admin@cinemaplus.local", "admin123");

        var addFilmPage = await client.GetStringAsync("/admin/films/add");
        var addFilmToken = TestHtml.ExtractAntiForgeryToken(addFilmPage);
        var filmContent = new MultipartFormDataContent
        {
            { new StringContent(addFilmToken), "__RequestVerificationToken" },
            { new StringContent("Інтеграційний тестовий фільм"), "Title" },
            { new StringContent("Фантастика"), "Genre" },
            { new StringContent("118"), "DurationMinutes" },
            { new StringContent("Опис тестового фільму для інтеграційної перевірки."), "Description" },
            { new StringContent("2026"), "ReleaseYear" },
            { new StringContent("12+"), "AgeRestriction" },
            { new StringContent("2026-09-15"), "AvailabilityDate" },
            { new StringContent("ComingSoon"), "AvailabilityStatus" }
        };

        var posterBytes = new byte[]
        {
            0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,
            0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
            0x08, 0x06, 0x00, 0x00, 0x00, 0x1F, 0x15, 0xC4,
            0x89, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x44, 0x41,
            0x54, 0x78, 0x9C, 0x63, 0x00, 0x01, 0x00, 0x00,
            0x05, 0x00, 0x01, 0x0D, 0x0A, 0x2D, 0xB4, 0x00,
            0x00, 0x00, 0x00, 0x49, 0x45, 0x4E, 0x44, 0xAE,
            0x42, 0x60, 0x82
        };

        var posterContent = new ByteArrayContent(posterBytes);
        posterContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
        filmContent.Add(posterContent, "PosterFile", "poster.png");

        var addFilmResponse = await client.PostAsync("/admin/films/add", filmContent);
        Assert.Equal(HttpStatusCode.Redirect, addFilmResponse.StatusCode);

        int filmId;
        string posterPath;
        await using (var db = factory.CreateDbContext())
        {
            var film = await db.Films.SingleAsync(item => item.Title == "Інтеграційний тестовий фільм");
            filmId = film.Id;
            posterPath = film.PosterPath;
            Assert.StartsWith("/uploads/", film.PosterPath);
        }

        var editFilmPage = await client.GetStringAsync($"/admin/films/edit/{filmId}");
        var editFilmToken = TestHtml.ExtractAntiForgeryToken(editFilmPage);
        var editFilmContent = new MultipartFormDataContent
        {
            { new StringContent(editFilmToken), "__RequestVerificationToken" },
            { new StringContent(filmId.ToString()), "Id" },
            { new StringContent("Інтеграційний тестовий фільм Оновлено"), "Title" },
            { new StringContent("Наукова фантастика"), "Genre" },
            { new StringContent("120"), "DurationMinutes" },
            { new StringContent("Оновлений опис тестового фільму."), "Description" },
            { new StringContent("2026"), "ReleaseYear" },
            { new StringContent("16+"), "AgeRestriction" },
            { new StringContent("2026-09-20"), "AvailabilityDate" },
            { new StringContent("ComingSoon"), "AvailabilityStatus" },
            { new StringContent(posterPath), "CurrentPosterPath" },
            { new StringContent(string.Empty), "CurrentTrailerPath" }
        };
        var editFilmResponse = await client.PostAsync($"/admin/films/edit/{filmId}", editFilmContent);
        Assert.True(editFilmResponse.StatusCode is HttpStatusCode.OK or HttpStatusCode.Redirect);

        var addHallPage = await client.GetStringAsync("/admin/halls/add");
        var addHallToken = TestHtml.ExtractAntiForgeryToken(addHallPage);
        var addHallResponse = await client.PostAsync(
            "/admin/halls/add",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = addHallToken,
                ["Name"] = "Інтеграційний зал",
                ["Technology"] = "4DX",
                ["RowsCount"] = "4",
                ["SeatsPerRow"] = "6"
            }));
        Assert.Equal(HttpStatusCode.Redirect, addHallResponse.StatusCode);

        int hallId;
        await using (var db = factory.CreateDbContext())
        {
            var hall = await db.Halls.SingleAsync(item => item.Name == "Інтеграційний зал");
            hallId = hall.Id;
            Assert.Equal(24, await db.Seats.CountAsync(item => item.HallId == hallId));
        }

        var editHallPage = await client.GetStringAsync($"/admin/halls/edit/{hallId}");
        var editHallToken = TestHtml.ExtractAntiForgeryToken(editHallPage);
        var editHallResponse = await client.PostAsync(
            $"/admin/halls/edit/{hallId}",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = editHallToken,
                ["Id"] = hallId.ToString(),
                ["Name"] = "Інтеграційний зал Оновлено",
                ["Technology"] = "IMAX",
                ["RowsCount"] = "5",
                ["SeatsPerRow"] = "5",
                ["HasProtectedBookings"] = "false"
            }));
        Assert.Equal(HttpStatusCode.Redirect, editHallResponse.StatusCode);

        int sessionId;
        await using (var db = factory.CreateDbContext())
        {
            var session = new Session
            {
                FilmId = filmId,
                HallId = hallId,
                SessionTime = new DateTime(2026, 6, 12, 18, 30, 0),
                Price = 275.50m
            };
            db.Sessions.Add(session);
            await db.SaveChangesAsync();
            sessionId = session.Id;
            Assert.Equal(275.50m, session.Price);
        }

        var editSessionPage = await client.GetStringAsync($"/admin/sessions/edit/{sessionId}");
        var editSessionToken = TestHtml.ExtractAntiForgeryToken(editSessionPage);
        var editSessionResponse = await client.PostAsync(
            $"/admin/sessions/edit/{sessionId}",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = editSessionToken,
                ["Id"] = sessionId.ToString(),
                ["FilmId"] = filmId.ToString(),
                ["HallId"] = hallId.ToString(),
                ["SessionTime"] = "2026-06-12T19:15",
                ["Price"] = "299.00",
                ["HasBookings"] = "false"
            }));
        Assert.True(editSessionResponse.StatusCode is HttpStatusCode.OK or HttpStatusCode.Redirect);

        int bookingId;
        await using (var db = factory.CreateDbContext())
        {
            var firstSeatId = await db.Seats
                .Where(item => item.HallId == hallId)
                .OrderBy(item => item.RowNumber)
                .ThenBy(item => item.SeatNumber)
                .Select(item => item.Id)
                .FirstAsync();

            var booking = new Booking
            {
                SessionId = sessionId,
                BookingDate = new DateTime(2026, 6, 10, 12, 0, 0, DateTimeKind.Utc),
                Status = "Paid",
                TotalPrice = 299.00m,
                TicketCode = "CP-ADMIN-TEST",
                CustomerName = "Адмін Тест",
                CustomerEmail = "admin.booking@test.local",
                CustomerPhone = "+380500000000"
            };

            db.Bookings.Add(booking);
            await db.SaveChangesAsync();
            db.BookedSeats.Add(new BookedSeat
            {
                BookingId = booking.Id,
                SessionId = sessionId,
                SeatId = firstSeatId
            });
            await db.SaveChangesAsync();
            bookingId = booking.Id;
        }

        var bookingsPage = await client.GetStringAsync("/admin/bookings");
        var bookingsToken = TestHtml.ExtractAntiForgeryToken(bookingsPage);
        var cancelBookingResponse = await client.PostAsync(
            $"/admin/bookings/cancel/{bookingId}",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["__RequestVerificationToken"] = bookingsToken
            }));
        Assert.Equal(HttpStatusCode.Redirect, cancelBookingResponse.StatusCode);

        await using (var db = factory.CreateDbContext())
        {
            var cancelledBooking = await db.Bookings.Include(item => item.BookedSeats).SingleAsync(item => item.Id == bookingId);
            Assert.Equal("Cancelled", cancelledBooking.Status);
            Assert.Empty(cancelledBooking.BookedSeats);
        }

        var excelResponse = await client.GetAsync("/admin/reports/stats.xlsx?startDate=2026-06-01&endDate=2026-06-30");
        Assert.Equal(HttpStatusCode.OK, excelResponse.StatusCode);
        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelResponse.Content.Headers.ContentType?.MediaType);
        Assert.True((await excelResponse.Content.ReadAsByteArrayAsync()).Length > 0);
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
