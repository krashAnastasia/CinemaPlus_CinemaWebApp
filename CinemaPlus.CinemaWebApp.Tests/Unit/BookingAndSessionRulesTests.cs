using System.Security.Claims;
using CinemaPlus.CinemaWebApp.Controllers;
using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Models;
using CinemaPlus.CinemaWebApp.Services;
using CinemaPlus.CinemaWebApp.Tests.Infrastructure;
using CinemaPlus.CinemaWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CinemaPlus.CinemaWebApp.Tests.Unit;

public class BookingAndSessionRulesTests
{
    [Fact]
    public async Task CheckoutPost_CreatesPaidBooking_WithCalculatedTotalPrice()
    {
        using var database = new SqliteTestDatabase();
        await using var db = database.CreateContext();

        var hall = new Hall
        {
            Name = $"Unit Hall {Guid.NewGuid():N}",
            Technology = "2D",
            RowsCount = 1,
            SeatsPerRow = 2
        };

        var film = new Film
        {
            Title = $"Unit Film {Guid.NewGuid():N}",
            Genre = "Драма",
            DurationMinutes = 110,
            Description = "Фільм для unit-тесту checkout.",
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
            new Seat { HallId = hall.Id, RowNumber = 1, SeatNumber = 2 });
        await db.SaveChangesAsync();

        var session = new Session
        {
            FilmId = film.Id,
            HallId = hall.Id,
            SessionTime = new DateTime(2026, 5, 20, 18, 0, 0),
            Price = 250m
        };

        db.Sessions.Add(session);
        await db.SaveChangesAsync();

        var seatIds = await db.Seats
            .Where(item => item.HallId == hall.Id)
            .OrderBy(item => item.SeatNumber)
            .Select(item => item.Id)
            .ToListAsync();

        var controller = new CheckoutController(db, TestControllerFactory.CreateEnvironment(), new FixedTicketService());
        TestControllerFactory.AttachContext(controller);

        var result = await controller.Index(new CheckoutSelectionViewModel
        {
            SessionId = session.Id,
            SelectedSeatIds = seatIds,
            FullName = "Unit Guest",
            Email = "unit.checkout@test.local",
            Phone = "+380631234567",
            CardNumber = "4242 4242 4242 4242",
            CardExpiry = "12/30",
            CardCvc = "123",
            PaymentMethod = "Card"
        });

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Details", redirect.ActionName);
        Assert.Equal("Tickets", redirect.ControllerName);

        var booking = await db.Bookings
            .Include(item => item.BookedSeats)
            .SingleAsync(item => item.CustomerEmail == "unit.checkout@test.local");

        Assert.Equal("Paid", booking.Status);
        Assert.Equal(500m, booking.TotalPrice);
        Assert.Equal(2, booking.BookedSeats.Count);
        Assert.Equal("CP-UNIT-0001", booking.TicketCode);
        Assert.Equal(1, await db.NotificationLogs.CountAsync(item => item.BookingId == booking.Id));
    }

    [Fact]
    public async Task SeatSelectionPost_RejectsBookedAndForeignHallSeats()
    {
        using var database = new SqliteTestDatabase();
        await using var db = database.CreateContext();

        var hall = new Hall
        {
            Name = $"Seat Hall A {Guid.NewGuid():N}",
            Technology = "2D",
            RowsCount = 1,
            SeatsPerRow = 2
        };
        var otherHall = new Hall
        {
            Name = $"Seat Hall B {Guid.NewGuid():N}",
            Technology = "3D",
            RowsCount = 1,
            SeatsPerRow = 1
        };
        var film = new Film
        {
            Title = $"Seat Film {Guid.NewGuid():N}",
            Genre = "Трилер",
            DurationMinutes = 95,
            Description = "Фільм для unit-тесту місць.",
            ReleaseYear = 2026,
            AgeRestriction = "16+",
            PosterPath = "/source/hero-img.jpg",
            AvailabilityDate = new DateOnly(2026, 5, 1),
            AvailabilityStatus = "NowShowing"
        };

        db.Halls.AddRange(hall, otherHall);
        db.Films.Add(film);
        await db.SaveChangesAsync();

        var seatA1 = new Seat { HallId = hall.Id, RowNumber = 1, SeatNumber = 1 };
        var seatA2 = new Seat { HallId = hall.Id, RowNumber = 1, SeatNumber = 2 };
        var seatB1 = new Seat { HallId = otherHall.Id, RowNumber = 1, SeatNumber = 1 };
        db.Seats.AddRange(seatA1, seatA2, seatB1);
        await db.SaveChangesAsync();

        var session = new Session
        {
            FilmId = film.Id,
            HallId = hall.Id,
            SessionTime = new DateTime(2026, 5, 21, 19, 0, 0),
            Price = 180m
        };
        db.Sessions.Add(session);
        await db.SaveChangesAsync();

        var booking = new Booking
        {
            SessionId = session.Id,
            BookingDate = DateTime.UtcNow,
            Status = "Paid",
            TotalPrice = 180m,
            TicketCode = "CP-SEAT-RULE",
            CustomerName = "Seat Rule",
            CustomerEmail = "seat.rule@test.local",
            CustomerPhone = "+380501010101"
        };
        db.Bookings.Add(booking);
        await db.SaveChangesAsync();
        db.BookedSeats.Add(new BookedSeat
        {
            BookingId = booking.Id,
            SessionId = session.Id,
            SeatId = seatA1.Id
        });
        await db.SaveChangesAsync();

        var controller = new SessionsController(db, TestControllerFactory.CreateEnvironment());
        TestControllerFactory.AttachContext(controller);

        var result = await controller.Seats(session.Id, new SeatSelectionPostViewModel
        {
            SelectedSeatIds = [seatA1.Id, seatB1.Id]
        });

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<SeatSelectionViewModel>(viewResult.Model);
        Assert.Contains(controller.ModelState[string.Empty]!.Errors, error => error.ErrorMessage.Contains("не належать до цього залу", StringComparison.Ordinal));
        Assert.Contains(controller.ModelState[string.Empty]!.Errors, error => error.ErrorMessage.Contains("вже недоступні", StringComparison.Ordinal));
        Assert.Empty(await db.Bookings.Where(item => item.CustomerEmail == "seat.selection@new.local").ToListAsync());
        Assert.DoesNotContain(model.SelectedSeatIds, id => id == seatA1.Id || id == seatB1.Id);
    }

    [Fact]
    public async Task AddSessionPost_RejectsDuplicateFilmHallAndDateTime()
    {
        using var database = new SqliteTestDatabase();
        await using var db = database.CreateContext();

        var hall = new Hall
        {
            Name = $"Duplicate Hall {Guid.NewGuid():N}",
            Technology = "IMAX",
            RowsCount = 5,
            SeatsPerRow = 5
        };
        var film = new Film
        {
            Title = $"Duplicate Film {Guid.NewGuid():N}",
            Genre = "Фантастика",
            DurationMinutes = 140,
            Description = "Фільм для unit-тесту дубліката сеансу.",
            ReleaseYear = 2026,
            AgeRestriction = "12+",
            PosterPath = "/source/hero-img.jpg",
            AvailabilityDate = new DateOnly(2026, 5, 1),
            AvailabilityStatus = "NowShowing"
        };

        db.Halls.Add(hall);
        db.Films.Add(film);
        await db.SaveChangesAsync();

        var sessionTime = new DateTime(2026, 5, 22, 20, 0, 0);
        db.Sessions.Add(new Session
        {
            FilmId = film.Id,
            HallId = hall.Id,
            SessionTime = sessionTime,
            Price = 300m
        });
        await db.SaveChangesAsync();

        var controller = new AdminController(
            db,
            new ConfigurationBuilder().Build(),
            TestControllerFactory.CreateEnvironment());
        TestControllerFactory.AttachContext(controller);

        var result = await controller.AddSession(new AdminSessionFormViewModel
        {
            FilmId = film.Id,
            HallId = hall.Id,
            SessionTime = sessionTime,
            Price = 320m
        });

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("SessionForm", viewResult.ViewName);
        Assert.Contains(controller.ModelState[string.Empty]!.Errors, error => error.ErrorMessage.Contains("вже існує", StringComparison.Ordinal));
        Assert.Equal(1, await db.Sessions.CountAsync(item => item.FilmId == film.Id && item.HallId == hall.Id && item.SessionTime == sessionTime));
    }

    [Fact]
    public async Task ProfileCancelBooking_MarksCancelled_AndReleasesSeats()
    {
        using var database = new SqliteTestDatabase();
        await using var db = database.CreateContext();

        var user = new User
        {
            FullName = "Unit Client",
            Email = "unit.client@test.local",
            Phone = "+380631110000",
            PasswordHash = new Pbkdf2PasswordService().HashPassword("unit123"),
            Role = "Client",
            CreatedAt = DateTime.UtcNow
        };
        var hall = new Hall
        {
            Name = $"Cancel Hall {Guid.NewGuid():N}",
            Technology = "2D",
            RowsCount = 1,
            SeatsPerRow = 2
        };
        var film = new Film
        {
            Title = $"Cancel Film {Guid.NewGuid():N}",
            Genre = "Драма",
            DurationMinutes = 101,
            Description = "Фільм для unit-тесту скасування.",
            ReleaseYear = 2026,
            AgeRestriction = "12+",
            PosterPath = "/source/hero-img.jpg",
            AvailabilityDate = new DateOnly(2026, 5, 1),
            AvailabilityStatus = "NowShowing"
        };

        db.Users.Add(user);
        db.Halls.Add(hall);
        db.Films.Add(film);
        await db.SaveChangesAsync();

        var seat1 = new Seat { HallId = hall.Id, RowNumber = 1, SeatNumber = 1 };
        var seat2 = new Seat { HallId = hall.Id, RowNumber = 1, SeatNumber = 2 };
        db.Seats.AddRange(seat1, seat2);
        await db.SaveChangesAsync();

        var session = new Session
        {
            FilmId = film.Id,
            HallId = hall.Id,
            SessionTime = new DateTime(2026, 5, 23, 17, 0, 0),
            Price = 190m
        };
        db.Sessions.Add(session);
        await db.SaveChangesAsync();

        var booking = new Booking
        {
            UserId = user.Id,
            SessionId = session.Id,
            BookingDate = DateTime.UtcNow,
            Status = "Paid",
            TotalPrice = 380m,
            TicketCode = "CP-CANCEL-UNIT",
            CustomerName = user.FullName,
            CustomerEmail = user.Email,
            CustomerPhone = user.Phone
        };
        db.Bookings.Add(booking);
        await db.SaveChangesAsync();

        db.BookedSeats.AddRange(
            new BookedSeat { BookingId = booking.Id, SessionId = session.Id, SeatId = seat1.Id },
            new BookedSeat { BookingId = booking.Id, SessionId = session.Id, SeatId = seat2.Id });
        await db.SaveChangesAsync();

        var controller = new ProfileController(db, TestControllerFactory.CreateEnvironment(), new BonusService(db));
        TestControllerFactory.AttachContext(controller, new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "Client")
        });

        var result = await controller.CancelBooking(booking.Id);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);

        var refreshedBooking = await db.Bookings.Include(item => item.BookedSeats).SingleAsync(item => item.Id == booking.Id);
        Assert.Equal("Cancelled", refreshedBooking.Status);
        Assert.Empty(refreshedBooking.BookedSeats);
        Assert.Equal(1, await db.NotificationLogs.CountAsync(item => item.BookingId == booking.Id));
    }
}
