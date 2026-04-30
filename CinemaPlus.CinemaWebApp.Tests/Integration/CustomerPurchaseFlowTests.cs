using System.Net;
using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Tests.Integration;

public class CustomerPurchaseFlowTests
{
    [Fact]
    public async Task GuestPurchaseFlow_CreatesBooking_ShowsTicket_AndLocksSeats()
    {
        await using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        int filmId;
        int sessionId;
        decimal ticketPrice;
        List<int> seatIds;

        await using (var db = factory.CreateDbContext())
        {
            var session = await db.Sessions
                .Include(item => item.Hall)
                    .ThenInclude(hall => hall.Seats)
                .Include(item => item.BookedSeats)
                .Include(item => item.Film)
                .Where(item => item.SessionTime <= CinemaPlus.CinemaWebApp.CinemaPresentationHelper.NowShowingCutoff)
                .OrderBy(item => item.Id)
                .FirstAsync(item => item.Hall.Seats.Count(seat => !item.BookedSeats.Select(booked => booked.SeatId).Contains(seat.Id)) >= 2);

            filmId = session.FilmId;
            sessionId = session.Id;
            ticketPrice = session.Price;
            var bookedSeatIds = session.BookedSeats.Select(item => item.SeatId).ToHashSet();
            seatIds = session.Hall.Seats
                .Where(seat => !bookedSeatIds.Contains(seat.Id))
                .OrderBy(seat => seat.RowNumber)
                .ThenBy(seat => seat.SeatNumber)
                .Take(2)
                .Select(seat => seat.Id)
                .ToList();
        }

        var homePage = await client.GetStringAsync("/");
        Assert.Contains("У КІНО", homePage);

        var detailsPage = await client.GetStringAsync($"/movies/{filmId}");
        Assert.Contains("КУПИТИ КВИТОК", detailsPage);

        var movieSessionsResponse = await client.GetAsync($"/movies/{filmId}/sessions?sessionId={sessionId}");
        Assert.Equal(HttpStatusCode.Redirect, movieSessionsResponse.StatusCode);
        Assert.Equal($"/sessions/{sessionId}/seats", movieSessionsResponse.Headers.Location?.OriginalString);

        var seatPage = await client.GetStringAsync($"/sessions/{sessionId}/seats");
        Assert.Contains("ОФОРМИТИ ЗАМОВЛЕННЯ", seatPage);
        var seatToken = TestHtml.ExtractAntiForgeryToken(seatPage);

        var seatSelectionPayload = new List<KeyValuePair<string, string>>
        {
            new("__RequestVerificationToken", seatToken)
        };
        seatSelectionPayload.AddRange(seatIds.Select(id => new KeyValuePair<string, string>("SelectedSeatIds", id.ToString())));

        var seatSelectionResponse = await client.PostAsync(
            $"/sessions/{sessionId}/seats",
            new FormUrlEncodedContent(seatSelectionPayload));
        Assert.Equal(HttpStatusCode.Redirect, seatSelectionResponse.StatusCode);
        var checkoutUrl = seatSelectionResponse.Headers.Location?.OriginalString;
        Assert.NotNull(checkoutUrl);

        var checkoutPage = await client.GetStringAsync(checkoutUrl!);
        Assert.Contains("ОФОРМЛЕННЯ ЗАМОВЛЕННЯ", checkoutPage);
        var checkoutToken = TestHtml.ExtractAntiForgeryToken(checkoutPage);

        var checkoutPayload = new List<KeyValuePair<string, string>>
        {
            new("__RequestVerificationToken", checkoutToken),
            new("SessionId", sessionId.ToString()),
            new("MovieId", filmId.ToString()),
            new("FullName", "Гість Інтеграційний"),
            new("Email", "guest.purchase@test.local"),
            new("Phone", "+380991112233"),
            new("CardNumber", "4242 4242 4242 4242"),
            new("CardExpiry", "12/30"),
            new("CardCvc", "123"),
            new("PaymentMethod", "Card")
        };
        checkoutPayload.AddRange(seatIds.Select(id => new KeyValuePair<string, string>("SelectedSeatIds", id.ToString())));

        var checkoutResponse = await client.PostAsync("/checkout", new FormUrlEncodedContent(checkoutPayload));
        Assert.Equal(HttpStatusCode.Redirect, checkoutResponse.StatusCode);
        var ticketUrl = checkoutResponse.Headers.Location?.OriginalString;
        Assert.NotNull(ticketUrl);
        Assert.Contains("/tickets/", ticketUrl);

        var ticketPage = await client.GetStringAsync(ticketUrl!);
        Assert.Contains("guest.purchase@test.local", ticketPage);
        Assert.Contains("КОД КВИТКА", ticketPage);

        int bookingId;
        string ticketCode;

        await using (var db = factory.CreateDbContext())
        {
            var booking = await db.Bookings
                .Include(item => item.BookedSeats)
                .OrderByDescending(item => item.Id)
                .FirstAsync(item => item.CustomerEmail == "guest.purchase@test.local");

            bookingId = booking.Id;
            ticketCode = booking.TicketCode;
            Assert.Equal("Paid", booking.Status);
            Assert.Equal(ticketPrice * seatIds.Count, booking.TotalPrice);
            Assert.Equal(2, booking.BookedSeats.Count);
        }

        var pdfResponse = await client.GetAsync($"/tickets/{bookingId}/pdf?ticketCode={ticketCode}");
        Assert.Equal(HttpStatusCode.OK, pdfResponse.StatusCode);
        Assert.Equal("application/pdf", pdfResponse.Content.Headers.ContentType?.MediaType);
        Assert.True((await pdfResponse.Content.ReadAsByteArrayAsync()).Length > 0);

        await using (var db = factory.CreateDbContext())
        {
            var lockedSeatIds = await db.BookedSeats
                .Where(item => item.BookingId == bookingId)
                .Select(item => item.SeatId)
                .ToListAsync();

            Assert.Equal(seatIds.OrderBy(id => id), lockedSeatIds.OrderBy(id => id));
        }
    }
}
