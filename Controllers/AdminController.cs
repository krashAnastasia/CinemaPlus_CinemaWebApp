using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Models;
using CinemaPlus.CinemaWebApp.Services;
using CinemaPlus.CinemaWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController(
    ApplicationDbContext dbContext,
    IConfiguration configuration,
    IWebHostEnvironment environment) : Controller
{
    private static readonly HashSet<string> AllowedFilmSortFields =
    [
        "film_title",
        "genre",
        "duration",
        "release_year",
        "age_restriction"
    ];

    private static readonly HashSet<string> AllowedSessionSortFields =
    [
        "session_time",
        "price",
        "hall",
        "film_title"
    ];

    private static readonly HashSet<string> AllowedBookingSortFields =
    [
        "booking_date",
        "seats",
        "customer_name",
        "session_time",
        "status",
        "total_price",
        "id"
    ];

    private static readonly HashSet<string> AllowedPosterExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    ];

    private static readonly HashSet<string> AllowedTrailerExtensions =
    [
        ".mp4",
        ".mov",
        ".webm",
        ".ogv"
    ];

    private const long MaxUploadedMediaBytes = 512L * 1024L * 1024L;

    [HttpGet("")]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Films));
    }

    [HttpGet("films")]
    public async Task<IActionResult> Films(
        string? q,
        string? genre,
        int? yearMin,
        int? yearMax,
        string? sortBy,
        string? sortDir)
    {
        SetAdminView("Фільми", "films");

        var safeSortBy = AllowedFilmSortFields.Contains(sortBy ?? string.Empty) ? sortBy! : "film_title";
        var safeSortDir = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";

        var filmsQuery = dbContext.Films.AsNoTracking();

        var normalizedQuery = q?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            filmsQuery = filmsQuery.Where(item => item.Title.Contains(normalizedQuery));
        }

        var normalizedGenre = genre?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(normalizedGenre))
        {
            filmsQuery = filmsQuery.Where(item => item.Genre == normalizedGenre);
        }

        if (yearMin.HasValue && yearMin.Value > 0)
        {
            filmsQuery = filmsQuery.Where(item => item.ReleaseYear >= yearMin.Value);
        }

        if (yearMax.HasValue && yearMax.Value > 0)
        {
            filmsQuery = filmsQuery.Where(item => item.ReleaseYear <= yearMax.Value);
        }

        filmsQuery = ApplyFilmSorting(filmsQuery, safeSortBy, safeSortDir);

        var films = await filmsQuery.ToListAsync();
        var genres = await dbContext.Films
            .AsNoTracking()
            .Select(item => item.Genre)
            .Distinct()
            .OrderBy(item => item)
            .ToListAsync();

        var viewModel = new AdminFilmListViewModel
        {
            SearchQuery = normalizedQuery,
            Genre = normalizedGenre,
            YearMin = yearMin is > 0 ? yearMin : null,
            YearMax = yearMax is > 0 ? yearMax : null,
            SortBy = safeSortBy,
            SortDir = safeSortDir,
            StatusMessage = TempData["AdminStatusMessage"] as string,
            ErrorMessage = TempData["AdminErrorMessage"] as string,
            Genres = genres,
            Films = films.Select(film => new AdminFilmListItemViewModel
            {
                Id = film.Id,
                Title = film.Title,
                Genre = film.Genre,
                DurationMinutes = film.DurationMinutes,
                Description = film.Description,
                ReleaseYear = film.ReleaseYear,
                AgeRestriction = film.AgeRestriction,
                PosterPath = CinemaPresentationHelper.ResolvePosterPath(film, environment)
            }).ToList()
        };

        return View("Films", viewModel);
    }

    [HttpGet("films/add")]
    public IActionResult AddFilm()
    {
        SetAdminView("Додати фільм", "films");
        return View("FilmForm", new AdminFilmFormViewModel
        {
            AvailabilityDate = DateOnly.FromDateTime(DateTime.Today),
            AvailabilityStatus = "NowShowing"
        });
    }

    [HttpPost("films/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddFilm(AdminFilmFormViewModel model)
    {
        SetAdminView("Додати фільм", "films");

        await ValidateFilmFormAsync(model);
        if (!ModelState.IsValid)
        {
            return View("FilmForm", model);
        }

        var posterPath = await SavePosterAsync(model.PosterFile, model.CurrentPosterPath);
        var trailerPath = await SaveTrailerAsync(model.TrailerFile, model.CurrentTrailerPath);

        var film = new Film
        {
            Title = model.Title.Trim(),
            Genre = model.Genre.Trim(),
            DurationMinutes = model.DurationMinutes,
            Description = model.Description.Trim(),
            ReleaseYear = model.ReleaseYear,
            AgeRestriction = model.AgeRestriction.Trim(),
            AvailabilityDate = model.AvailabilityDate,
            AvailabilityStatus = model.AvailabilityStatus,
            PosterPath = posterPath,
            TrailerPath = trailerPath
        };

        dbContext.Films.Add(film);
        await dbContext.SaveChangesAsync();

        TempData["AdminStatusMessage"] = $"Фільм «{film.Title}» успішно додано.";
        return RedirectToAction(nameof(Films));
    }

    [HttpGet("films/edit/{id:int}")]
    public async Task<IActionResult> EditFilm(int id)
    {
        SetAdminView("Редагувати фільм", "films");

        var film = await dbContext.Films
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        if (film is null)
        {
            return NotFound();
        }

        return View("FilmForm", new AdminFilmFormViewModel
        {
            Id = film.Id,
            Title = film.Title,
            Genre = film.Genre,
            DurationMinutes = film.DurationMinutes,
            Description = film.Description,
            ReleaseYear = film.ReleaseYear,
            AgeRestriction = film.AgeRestriction,
            AvailabilityDate = film.AvailabilityDate,
            AvailabilityStatus = film.AvailabilityStatus,
            CurrentPosterPath = CinemaPresentationHelper.ResolvePosterPath(film, environment),
            CurrentTrailerPath = film.TrailerPath ?? string.Empty
        });
    }

    [HttpPost("films/edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFilm(int id, AdminFilmFormViewModel model)
    {
        SetAdminView("Редагувати фільм", "films");
        model.Id = id;

        var film = await dbContext.Films.FirstOrDefaultAsync(item => item.Id == id);
        if (film is null)
        {
            return NotFound();
        }

        model.CurrentPosterPath = CinemaPresentationHelper.ResolvePosterPath(film, environment);
        model.CurrentTrailerPath = film.TrailerPath ?? string.Empty;

        await ValidateFilmFormAsync(model);
        if (!ModelState.IsValid)
        {
            return View("FilmForm", model);
        }

        var previousPosterPath = film.PosterPath;
        var previousTrailerPath = film.TrailerPath;

        film.Title = model.Title.Trim();
        film.Genre = model.Genre.Trim();
        film.DurationMinutes = model.DurationMinutes;
        film.Description = model.Description.Trim();
        film.ReleaseYear = model.ReleaseYear;
        film.AgeRestriction = model.AgeRestriction.Trim();
        film.AvailabilityDate = model.AvailabilityDate;
        film.AvailabilityStatus = model.AvailabilityStatus;
        film.PosterPath = await SavePosterAsync(model.PosterFile, film.PosterPath);
        film.TrailerPath = await SaveTrailerAsync(model.TrailerFile, film.TrailerPath);

        await dbContext.SaveChangesAsync();

        if (!string.Equals(previousPosterPath, film.PosterPath, StringComparison.Ordinal))
        {
            DeleteManagedPoster(previousPosterPath);
        }

        if (!string.Equals(previousTrailerPath, film.TrailerPath, StringComparison.Ordinal))
        {
            DeleteManagedTrailer(previousTrailerPath);
        }

        TempData["AdminStatusMessage"] = $"Фільм «{film.Title}» успішно оновлено.";
        return RedirectToAction(nameof(Films));
    }

    [HttpPost("films/delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFilm(int id)
    {
        var film = await dbContext.Films
            .Include(item => item.Sessions)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (film is null)
        {
            return NotFound();
        }

        if (film.Sessions.Count > 0)
        {
            TempData["AdminErrorMessage"] = $"Фільм «{film.Title}» не можна видалити, доки для нього існують сеанси.";
            return RedirectToAction(nameof(Films));
        }

        var posterPath = film.PosterPath;
        var trailerPath = film.TrailerPath;
        dbContext.Films.Remove(film);
        await dbContext.SaveChangesAsync();
        DeleteManagedPoster(posterPath);
        DeleteManagedTrailer(trailerPath);

        TempData["AdminStatusMessage"] = $"Фільм «{film.Title}» успішно видалено.";
        return RedirectToAction(nameof(Films));
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> Sessions(
        int? hallId,
        decimal? priceMin,
        decimal? priceMax,
        DateTime? startDate,
        DateTime? endDate,
        string? sortBy,
        string? sortDir)
    {
        SetAdminView("Сеанси", "sessions");

        var safeSortBy = AllowedSessionSortFields.Contains(sortBy ?? string.Empty) ? sortBy! : "session_time";
        var safeSortDir = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";
        var filteredHallId = hallId is > 0 ? hallId : null;
        var normalizedStartDate = startDate?.Date;
        var normalizedEndDate = endDate?.Date;

        var sessionsQuery = dbContext.Sessions
            .AsNoTracking()
            .AsSplitQuery()
            .Include(item => item.Film)
            .Include(item => item.Hall)
            .Include(item => item.Bookings)
                .ThenInclude(booking => booking.BookedSeats)
            .AsQueryable();

        if (filteredHallId.HasValue)
        {
            sessionsQuery = sessionsQuery.Where(item => item.HallId == filteredHallId.Value);
        }

        if (priceMin.HasValue)
        {
            sessionsQuery = sessionsQuery.Where(item => item.Price >= priceMin.Value);
        }

        if (priceMax.HasValue)
        {
            sessionsQuery = sessionsQuery.Where(item => item.Price <= priceMax.Value);
        }

        if (normalizedStartDate.HasValue)
        {
            sessionsQuery = sessionsQuery.Where(item => item.SessionTime >= normalizedStartDate.Value);
        }

        if (normalizedEndDate.HasValue)
        {
            sessionsQuery = sessionsQuery.Where(item => item.SessionTime < normalizedEndDate.Value.AddDays(1));
        }

        sessionsQuery = ApplySessionSorting(sessionsQuery, safeSortBy, safeSortDir);

        var sessions = await sessionsQuery.ToListAsync();
        var halls = await dbContext.Halls
            .AsNoTracking()
            .OrderBy(item => item.Name)
            .ToListAsync();

        var viewModel = new AdminSessionListViewModel
        {
            HallId = filteredHallId,
            PriceMin = priceMin,
            PriceMax = priceMax,
            StartDate = normalizedStartDate,
            EndDate = normalizedEndDate,
            SortBy = safeSortBy,
            SortDir = safeSortDir,
            StatusMessage = TempData["AdminStatusMessage"] as string,
            ErrorMessage = TempData["AdminErrorMessage"] as string,
            Halls = halls.Select(hall => new AdminSessionHallOptionViewModel
            {
                Id = hall.Id,
                Name = hall.Name,
                Technology = hall.Technology
            }).ToList(),
            Sessions = sessions.Select(session =>
            {
                var paidBookings = session.Bookings.Where(booking => booking.Status != "Cancelled").ToList();

                return new AdminSessionListItemViewModel
                {
                    Id = session.Id,
                    FilmTitle = session.Film.Title,
                    SessionTime = session.SessionTime,
                    HallName = session.Hall.Name,
                    Technology = session.Hall.Technology,
                    Price = session.Price,
                    BookingsCount = paidBookings.Count,
                    SoldSeatsCount = paidBookings.Sum(booking => booking.BookedSeats.Count)
                };
            }).ToList()
        };

        return View("Sessions", viewModel);
    }

    [HttpGet("sessions/add")]
    public async Task<IActionResult> AddSession()
    {
        SetAdminView("Додати новий сеанс", "sessions");

        var model = new AdminSessionFormViewModel
        {
            SessionTime = DateTime.Today.AddDays(1).AddHours(18),
            Price = 220m
        };

        await PopulateSessionFormOptionsAsync(model);
        return View("SessionForm", model);
    }

    [HttpPost("sessions/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSession(AdminSessionFormViewModel model)
    {
        SetAdminView("Додати новий сеанс", "sessions");

        await ValidateSessionFormAsync(model);
        if (!ModelState.IsValid)
        {
            await PopulateSessionFormOptionsAsync(model);
            return View("SessionForm", model);
        }

        var session = new Session
        {
            FilmId = model.FilmId!.Value,
            HallId = model.HallId!.Value,
            SessionTime = model.SessionTime!.Value,
            Price = model.Price!.Value
        };

        dbContext.Sessions.Add(session);
        await dbContext.SaveChangesAsync();

        var filmTitle = await dbContext.Films
            .Where(item => item.Id == session.FilmId)
            .Select(item => item.Title)
            .FirstAsync();

        TempData["AdminStatusMessage"] = $"Сеанс для фільму «{filmTitle}» успішно додано.";
        return RedirectToAction(nameof(Sessions));
    }

    [HttpGet("sessions/edit/{id:int}")]
    public async Task<IActionResult> EditSession(int id)
    {
        SetAdminView("Редагувати сеанс", "sessions");

        var session = await dbContext.Sessions
            .AsNoTracking()
            .Include(item => item.Bookings)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (session is null)
        {
            return NotFound();
        }

        var model = new AdminSessionFormViewModel
        {
            Id = session.Id,
            FilmId = session.FilmId,
            HallId = session.HallId,
            SessionTime = session.SessionTime,
            Price = session.Price,
            HasBookings = session.Bookings.Count > 0
        };

        await PopulateSessionFormOptionsAsync(model);
        return View("SessionForm", model);
    }

    [HttpPost("sessions/edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditSession(int id, AdminSessionFormViewModel model)
    {
        SetAdminView("Редагувати сеанс", "sessions");
        model.Id = id;

        var session = await dbContext.Sessions
            .Include(item => item.Bookings)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (session is null)
        {
            return NotFound();
        }

        model.HasBookings = session.Bookings.Count > 0;
        await ValidateSessionFormAsync(model);

        if (model.HasBookings && (session.FilmId != model.FilmId || session.HallId != model.HallId))
        {
            ModelState.AddModelError(string.Empty, "Сеанс із наявними бронюваннями не можна переносити на інший фільм або зал. Змініть лише час або ціну.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateSessionFormOptionsAsync(model);
            return View("SessionForm", model);
        }

        session.FilmId = model.FilmId!.Value;
        session.HallId = model.HallId!.Value;
        session.SessionTime = model.SessionTime!.Value;
        session.Price = model.Price!.Value;

        await dbContext.SaveChangesAsync();

        var filmTitle = await dbContext.Films
            .Where(item => item.Id == session.FilmId)
            .Select(item => item.Title)
            .FirstAsync();

        TempData["AdminStatusMessage"] = $"Сеанс для фільму «{filmTitle}» успішно оновлено.";
        return RedirectToAction(nameof(Sessions));
    }

    [HttpPost("sessions/delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSession(int id)
    {
        var session = await dbContext.Sessions
            .Include(item => item.Film)
            .Include(item => item.Bookings)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (session is null)
        {
            return NotFound();
        }

        if (session.Bookings.Count > 0)
        {
            TempData["AdminErrorMessage"] = "Сеанс не можна видалити, доки для нього існують бронювання. За потреби спершу скасуйте бронювання.";
            return RedirectToAction(nameof(Sessions));
        }

        dbContext.Sessions.Remove(session);
        await dbContext.SaveChangesAsync();

        TempData["AdminStatusMessage"] = $"Сеанс для фільму «{session.Film.Title}» успішно видалено.";
        return RedirectToAction(nameof(Sessions));
    }

    [HttpGet("halls")]
    public async Task<IActionResult> Halls()
    {
        SetAdminView("Зали", "halls");

        var halls = await dbContext.Halls
            .AsNoTracking()
            .Include(item => item.Seats)
            .Include(item => item.Sessions)
                .ThenInclude(session => session.BookedSeats)
            .OrderBy(item => item.Name)
            .ToListAsync();

        var viewModel = new AdminHallListViewModel
        {
            StatusMessage = TempData["AdminStatusMessage"] as string,
            ErrorMessage = TempData["AdminErrorMessage"] as string,
            Halls = halls.Select(hall => new AdminHallListItemViewModel
            {
                Id = hall.Id,
                Name = hall.Name,
                Technology = hall.Technology,
                RowsCount = hall.RowsCount,
                SeatsPerRow = hall.SeatsPerRow,
                TotalSeats = hall.Seats.Count,
                SessionsCount = hall.Sessions.Count,
                LayoutLocked = hall.Sessions.Any(session => session.BookedSeats.Count > 0)
            }).ToList()
        };

        return View("Halls", viewModel);
    }

    [HttpGet("halls/add")]
    public IActionResult AddHall()
    {
        SetAdminView("Додати зал", "halls");
        return View("HallForm", new AdminHallFormViewModel
        {
            RowsCount = 6,
            SeatsPerRow = 10,
            Technology = "2D"
        });
    }

    [HttpPost("halls/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddHall(AdminHallFormViewModel model)
    {
        SetAdminView("Додати зал", "halls");

        await ValidateHallFormAsync(model);
        if (!ModelState.IsValid)
        {
            return View("HallForm", model);
        }

        var hall = new Hall
        {
            Name = model.Name.Trim(),
            Technology = model.Technology.Trim(),
            RowsCount = model.RowsCount,
            SeatsPerRow = model.SeatsPerRow
        };

        dbContext.Halls.Add(hall);
        await dbContext.SaveChangesAsync();

        dbContext.Seats.AddRange(BuildSeatLayout(hall.Id, model.RowsCount, model.SeatsPerRow));
        await dbContext.SaveChangesAsync();

        TempData["AdminStatusMessage"] = $"Зал «{hall.Name}» успішно створено.";
        return RedirectToAction(nameof(Halls));
    }

    [HttpGet("halls/edit/{id:int}")]
    public async Task<IActionResult> EditHall(int id)
    {
        SetAdminView("Редагувати зал", "halls");

        var hall = await dbContext.Halls
            .AsNoTracking()
            .Include(item => item.Sessions)
                .ThenInclude(session => session.BookedSeats)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (hall is null)
        {
            return NotFound();
        }

        return View("HallForm", new AdminHallFormViewModel
        {
            Id = hall.Id,
            Name = hall.Name,
            Technology = hall.Technology,
            RowsCount = hall.RowsCount,
            SeatsPerRow = hall.SeatsPerRow,
            HasProtectedBookings = hall.Sessions.Any(session => session.BookedSeats.Count > 0)
        });
    }

    [HttpPost("halls/edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditHall(int id, AdminHallFormViewModel model)
    {
        SetAdminView("Редагувати зал", "halls");
        model.Id = id;

        var hall = await dbContext.Halls
            .Include(item => item.Seats)
            .Include(item => item.Sessions)
                .ThenInclude(session => session.BookedSeats)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (hall is null)
        {
            return NotFound();
        }

        var hasProtectedBookings = hall.Sessions.Any(session => session.BookedSeats.Count > 0);
        model.HasProtectedBookings = hasProtectedBookings;

        await ValidateHallFormAsync(model);

        var layoutChanged = hall.RowsCount != model.RowsCount || hall.SeatsPerRow != model.SeatsPerRow;
        if (layoutChanged && hasProtectedBookings)
        {
            ModelState.AddModelError(string.Empty, "Не можна змінити схему залу, поки в ньому є заброньовані місця. Оновіть тільки назву або технологію.");
        }

        if (!ModelState.IsValid)
        {
            return View("HallForm", model);
        }

        hall.Name = model.Name.Trim();
        hall.Technology = model.Technology.Trim();

        if (layoutChanged)
        {
            hall.RowsCount = model.RowsCount;
            hall.SeatsPerRow = model.SeatsPerRow;
            dbContext.Seats.RemoveRange(hall.Seats);
            dbContext.Seats.AddRange(BuildSeatLayout(hall.Id, model.RowsCount, model.SeatsPerRow));
        }

        await dbContext.SaveChangesAsync();

        TempData["AdminStatusMessage"] = $"Параметри залу «{hall.Name}» успішно оновлено.";
        return RedirectToAction(nameof(Halls));
    }

    [HttpPost("halls/delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteHall(int id)
    {
        var hall = await dbContext.Halls
            .Include(item => item.Sessions)
            .Include(item => item.Seats)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (hall is null)
        {
            return NotFound();
        }

        if (hall.Sessions.Count > 0)
        {
            TempData["AdminErrorMessage"] = $"Зал «{hall.Name}» не можна видалити, доки для нього існують сеанси.";
            return RedirectToAction(nameof(Halls));
        }

        dbContext.Halls.Remove(hall);
        await dbContext.SaveChangesAsync();

        TempData["AdminStatusMessage"] = $"Зал «{hall.Name}» успішно видалено.";
        return RedirectToAction(nameof(Halls));
    }

    [HttpGet("bookings")]
    public async Task<IActionResult> Bookings(
        string? q,
        string? mode,
        string? status,
        string? sortBy,
        string? sortDir)
    {
        SetAdminView("Бронювання", "bookings");

        var normalizedQuery = q?.Trim() ?? string.Empty;
        var safeMode = string.Equals(mode, "exact", StringComparison.OrdinalIgnoreCase) ? "exact" : "partial";
        var normalizedStatus = status?.Trim() ?? string.Empty;
        var safeSortBy = AllowedBookingSortFields.Contains(sortBy ?? string.Empty) ? sortBy! : "booking_date";
        var safeSortDir = string.Equals(sortDir, "asc", StringComparison.OrdinalIgnoreCase) ? "asc" : "desc";

        var bookingsQuery = dbContext.Bookings
            .AsNoTracking()
            .AsSplitQuery()
            .Include(item => item.Session)
                .ThenInclude(session => session.Film)
            .Include(item => item.Session)
                .ThenInclude(session => session.Hall)
            .Include(item => item.BookedSeats)
                .ThenInclude(bookedSeat => bookedSeat.Seat)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(normalizedQuery))
        {
            bookingsQuery = safeMode == "exact"
                ? bookingsQuery.Where(item => item.CustomerName == normalizedQuery || item.CustomerEmail == normalizedQuery)
                : bookingsQuery.Where(item => item.CustomerName.Contains(normalizedQuery) || item.CustomerEmail.Contains(normalizedQuery));
        }

        if (!string.IsNullOrWhiteSpace(normalizedStatus))
        {
            bookingsQuery = bookingsQuery.Where(item => item.Status == normalizedStatus);
        }

        bookingsQuery = ApplyBookingSorting(bookingsQuery, safeSortBy, safeSortDir);

        var bookings = await bookingsQuery.ToListAsync();
        var availableStatuses = await dbContext.Bookings
            .AsNoTracking()
            .Select(item => item.Status)
            .Distinct()
            .OrderBy(item => item)
            .ToListAsync();

        var viewModel = new AdminBookingListViewModel
        {
            SearchQuery = normalizedQuery,
            SearchMode = safeMode,
            StatusFilter = normalizedStatus,
            SortBy = safeSortBy,
            SortDir = safeSortDir,
            StatusMessage = TempData["AdminStatusMessage"] as string,
            ErrorMessage = TempData["AdminErrorMessage"] as string,
            AvailableStatuses = availableStatuses,
            Bookings = bookings.Select(booking =>
            {
                var orderedSeats = booking.BookedSeats
                    .Select(item => item.Seat)
                    .OrderBy(seat => seat.RowNumber)
                    .ThenBy(seat => seat.SeatNumber)
                    .ToList();

                return new AdminBookingListItemViewModel
                {
                    Id = booking.Id,
                    SessionId = booking.SessionId,
                    CustomerName = booking.CustomerName,
                    CustomerEmail = booking.CustomerEmail,
                    CustomerPhone = booking.CustomerPhone,
                    SessionSummary = BuildSessionSummary(booking.Session),
                    SeatSummary = orderedSeats.Count > 0
                        ? string.Join(" • ", orderedSeats.Select(seat => CinemaPresentationHelper.FormatSeatLabel(seat.RowNumber, seat.SeatNumber)))
                        : "Місця звільнено",
                    SeatsCount = orderedSeats.Count,
                    Status = booking.Status,
                    BookingDate = booking.BookingDate,
                    TotalPrice = booking.TotalPrice,
                    CanCancel = booking.Status != "Cancelled"
                };
            }).ToList()
        };

        return View("Bookings", viewModel);
    }

    [HttpPost("bookings/cancel/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var booking = await dbContext.Bookings
            .Include(item => item.BookedSeats)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (booking is null)
        {
            return NotFound();
        }

        if (booking.Status == "Cancelled")
        {
            TempData["AdminStatusMessage"] = $"Бронювання №{booking.Id} уже скасовано.";
            return RedirectToAction(nameof(Bookings));
        }

        booking.Status = "Cancelled";
        dbContext.BookedSeats.RemoveRange(booking.BookedSeats);
        dbContext.NotificationLogs.Add(new NotificationLog
        {
            BookingId = booking.Id,
            Email = booking.CustomerEmail,
            CreatedDate = DateTime.UtcNow,
            Status = "Emulated",
            Message = $"Адміністратор скасував бронювання №{booking.Id}. Місця знову доступні для продажу."
        });

        await dbContext.SaveChangesAsync();
        TempData["AdminStatusMessage"] = $"Бронювання №{booking.Id} успішно скасовано.";
        return RedirectToAction(nameof(Bookings));
    }

    [HttpGet("reports")]
    public async Task<IActionResult> Reports(DateTime? startDate, DateTime? endDate)
    {
        SetAdminView("Звіти", "reports");

        var viewModel = await BuildReportsViewModelAsync(startDate, endDate);
        viewModel.StatusMessage ??= TempData["AdminStatusMessage"] as string;
        viewModel.ErrorMessage ??= TempData["AdminErrorMessage"] as string;

        return View("Reports", viewModel);
    }

    [HttpGet("reports/stats.xlsx")]
    public async Task<IActionResult> ExportStatsExcel(DateTime? startDate, DateTime? endDate)
    {
        var dateRange = await ResolveReportDateRangeAsync(startDate, endDate);
        if (!string.IsNullOrWhiteSpace(dateRange.ErrorMessage))
        {
            TempData["AdminErrorMessage"] = dateRange.ErrorMessage;
            return RedirectToAction(nameof(Reports), new { startDate, endDate });
        }

        var sessions = await LoadSessionsForReportRangeAsync(dateRange.StartDate, dateRange.EndDate);
        var workbook = BuildExcelWorkbook(sessions);

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm", CinemaPresentationHelper.UkrainianCulture);
        var fileName = $"Stats_{timestamp}.xlsx";
        await PersistExcelReportAsync(fileName, workbook);

        return File(
            workbook,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }

    [HttpGet("stats")]
    public IActionResult Stats()
    {
        return RedirectToAction(nameof(PopularFilms));
    }

    [HttpGet("stats/popular-films")]
    public async Task<IActionResult> PopularFilms()
    {
        SetAdminView("Популярність фільмів", "stats");

        var films = await dbContext.Films
            .AsNoTracking()
            .AsSplitQuery()
            .Include(item => item.Sessions)
                .ThenInclude(session => session.Bookings)
                    .ThenInclude(booking => booking.BookedSeats)
            .OrderBy(item => item.Title)
            .ToListAsync();

        var viewModel = new AdminPopularFilmsStatsViewModel
        {
            Items = films
                .Select(film =>
                {
                    var paidBookings = film.Sessions
                        .SelectMany(session => session.Bookings)
                        .Where(booking => booking.Status != "Cancelled")
                        .ToList();

                    return new AdminPopularFilmsStatsItemViewModel
                    {
                        FilmTitle = film.Title,
                        Genre = film.Genre,
                        SessionsCount = film.Sessions.Count,
                        BookingsCount = paidBookings.Count,
                        SoldSeatsCount = paidBookings.Sum(booking => booking.BookedSeats.Count)
                    };
                })
                .OrderByDescending(item => item.SoldSeatsCount)
                .ThenByDescending(item => item.BookingsCount)
                .ThenByDescending(item => item.SessionsCount)
                .ThenBy(item => item.FilmTitle)
                .ToList()
        };

        return View("PopularFilms", viewModel);
    }

    [HttpGet("stats/revenue")]
    public async Task<IActionResult> Revenue(DateTime? startDate, DateTime? endDate)
    {
        SetAdminView("Доходи за період", "stats");

        var viewModel = await BuildRevenueViewModelAsync(startDate, endDate);
        return View("Revenue", viewModel);
    }

    private void SetAdminView(string title, string section)
    {
        ViewData["Title"] = title;
        ViewData["AdminSection"] = section;
    }

    private static IQueryable<Film> ApplyFilmSorting(IQueryable<Film> query, string sortBy, string sortDir)
    {
        var descending = sortDir == "desc";

        return (sortBy, descending) switch
        {
            ("genre", true) => query.OrderByDescending(item => item.Genre).ThenBy(item => item.Title),
            ("genre", false) => query.OrderBy(item => item.Genre).ThenBy(item => item.Title),
            ("duration", true) => query.OrderByDescending(item => item.DurationMinutes).ThenBy(item => item.Title),
            ("duration", false) => query.OrderBy(item => item.DurationMinutes).ThenBy(item => item.Title),
            ("release_year", true) => query.OrderByDescending(item => item.ReleaseYear).ThenBy(item => item.Title),
            ("release_year", false) => query.OrderBy(item => item.ReleaseYear).ThenBy(item => item.Title),
            ("age_restriction", true) => query.OrderByDescending(item => item.AgeRestriction).ThenBy(item => item.Title),
            ("age_restriction", false) => query.OrderBy(item => item.AgeRestriction).ThenBy(item => item.Title),
            ("film_title", true) => query.OrderByDescending(item => item.Title),
            _ => query.OrderBy(item => item.Title)
        };
    }

    private static IQueryable<Session> ApplySessionSorting(IQueryable<Session> query, string sortBy, string sortDir)
    {
        var descending = sortDir == "desc";

        return (sortBy, descending) switch
        {
            ("price", true) => query.OrderByDescending(item => item.Price).ThenBy(item => item.SessionTime),
            ("price", false) => query.OrderBy(item => item.Price).ThenBy(item => item.SessionTime),
            ("hall", true) => query.OrderByDescending(item => item.Hall.Name).ThenBy(item => item.SessionTime),
            ("hall", false) => query.OrderBy(item => item.Hall.Name).ThenBy(item => item.SessionTime),
            ("film_title", true) => query.OrderByDescending(item => item.Film.Title).ThenBy(item => item.SessionTime),
            ("film_title", false) => query.OrderBy(item => item.Film.Title).ThenBy(item => item.SessionTime),
            ("session_time", true) => query.OrderByDescending(item => item.SessionTime),
            _ => query.OrderBy(item => item.SessionTime)
        };
    }

    private static IQueryable<Booking> ApplyBookingSorting(IQueryable<Booking> query, string sortBy, string sortDir)
    {
        var descending = sortDir == "desc";

        return (sortBy, descending) switch
        {
            ("seats", true) => query.OrderByDescending(item => item.BookedSeats.Count).ThenByDescending(item => item.BookingDate),
            ("seats", false) => query.OrderBy(item => item.BookedSeats.Count).ThenByDescending(item => item.BookingDate),
            ("customer_name", true) => query.OrderByDescending(item => item.CustomerName).ThenByDescending(item => item.BookingDate),
            ("customer_name", false) => query.OrderBy(item => item.CustomerName).ThenByDescending(item => item.BookingDate),
            ("session_time", true) => query.OrderByDescending(item => item.Session.SessionTime).ThenByDescending(item => item.BookingDate),
            ("session_time", false) => query.OrderBy(item => item.Session.SessionTime).ThenByDescending(item => item.BookingDate),
            ("status", true) => query.OrderByDescending(item => item.Status).ThenByDescending(item => item.BookingDate),
            ("status", false) => query.OrderBy(item => item.Status).ThenByDescending(item => item.BookingDate),
            ("total_price", true) => query.OrderByDescending(item => item.TotalPrice).ThenByDescending(item => item.BookingDate),
            ("total_price", false) => query.OrderBy(item => item.TotalPrice).ThenByDescending(item => item.BookingDate),
            ("id", false) => query.OrderBy(item => item.Id),
            ("id", true) => query.OrderByDescending(item => item.Id),
            ("booking_date", false) => query.OrderBy(item => item.BookingDate),
            _ => query.OrderByDescending(item => item.BookingDate)
        };
    }

    private async Task ValidateFilmFormAsync(AdminFilmFormViewModel model)
    {
        var normalizedTitle = model.Title.Trim();

        if (await dbContext.Films.AnyAsync(item => item.Title == normalizedTitle && item.Id != model.Id))
        {
            ModelState.AddModelError(nameof(model.Title), "Фільм із такою назвою вже існує.");
        }

        if (model.PosterFile is not null)
        {
            var extension = Path.GetExtension(model.PosterFile.FileName).ToLowerInvariant();
            if (!AllowedPosterExtensions.Contains(extension))
            {
                ModelState.AddModelError(nameof(model.PosterFile), "Дозволено лише зображення JPG, PNG або WebP.");
            }

            if (model.PosterFile.Length == 0)
            {
                ModelState.AddModelError(nameof(model.PosterFile), "Файл постера порожній. Оберіть інший файл.");
            }

            if (model.PosterFile.Length > MaxUploadedMediaBytes)
            {
                ModelState.AddModelError(nameof(model.PosterFile), "Файл постера перевищує ліміт у 512 МБ.");
            }
        }

        if (model.TrailerFile is not null)
        {
            var extension = Path.GetExtension(model.TrailerFile.FileName).ToLowerInvariant();
            if (!AllowedTrailerExtensions.Contains(extension))
            {
                ModelState.AddModelError(nameof(model.TrailerFile), "Дозволено лише відеофайли MP4, MOV, WebM або OGV.");
            }

            if (model.TrailerFile.Length == 0)
            {
                ModelState.AddModelError(nameof(model.TrailerFile), "Файл трейлера порожній. Оберіть інший файл.");
            }

            if (model.TrailerFile.Length > MaxUploadedMediaBytes)
            {
                ModelState.AddModelError(nameof(model.TrailerFile), "Файл трейлера перевищує ліміт у 512 МБ.");
            }
        }
    }

    private async Task ValidateHallFormAsync(AdminHallFormViewModel model)
    {
        var normalizedName = model.Name.Trim();
        if (await dbContext.Halls.AnyAsync(item => item.Name == normalizedName && item.Id != model.Id))
        {
            ModelState.AddModelError(nameof(model.Name), "Зал із такою назвою вже існує.");
        }
    }

    private async Task ValidateSessionFormAsync(AdminSessionFormViewModel model)
    {
        if (model.FilmId.HasValue && !await dbContext.Films.AnyAsync(item => item.Id == model.FilmId.Value))
        {
            ModelState.AddModelError(nameof(model.FilmId), "Оберіть фільм зі списку.");
        }

        if (model.HallId.HasValue && !await dbContext.Halls.AnyAsync(item => item.Id == model.HallId.Value))
        {
            ModelState.AddModelError(nameof(model.HallId), "Оберіть зал зі списку.");
        }

        if (!model.FilmId.HasValue || !model.HallId.HasValue || !model.SessionTime.HasValue)
        {
            return;
        }

        var duplicateExists = await dbContext.Sessions.AnyAsync(item =>
            item.Id != model.Id
            && item.FilmId == model.FilmId.Value
            && item.HallId == model.HallId.Value
            && item.SessionTime == model.SessionTime.Value);

        if (duplicateExists)
        {
            ModelState.AddModelError(string.Empty, "Сеанс із таким фільмом, залом і датою вже існує. Оберіть інший час.");
        }
    }

    private async Task PopulateSessionFormOptionsAsync(AdminSessionFormViewModel model)
    {
        model.Films = await dbContext.Films
            .AsNoTracking()
            .OrderBy(item => item.Title)
            .Select(item => new AdminSessionOptionViewModel
            {
                Id = item.Id,
                Label = $"{item.Title} ({item.ReleaseYear})"
            })
            .ToListAsync();

        model.Halls = await dbContext.Halls
            .AsNoTracking()
            .OrderBy(item => item.Name)
            .Select(item => new AdminSessionOptionViewModel
            {
                Id = item.Id,
                Label = $"{item.Name} • {item.Technology} • {item.RowsCount}x{item.SeatsPerRow}"
            })
            .ToListAsync();
    }

    private async Task<AdminRevenueViewModel> BuildRevenueViewModelAsync(DateTime? startDate, DateTime? endDate)
    {
        var dateRange = await ResolveReportDateRangeAsync(startDate, endDate);

        var viewModel = new AdminRevenueViewModel
        {
            ErrorMessage = dateRange.ErrorMessage,
            StartDate = dateRange.StartDate,
            EndDate = dateRange.EndDate
        };

        if (!string.IsNullOrWhiteSpace(dateRange.ErrorMessage))
        {
            return viewModel;
        }

        var sessions = await LoadSessionsForReportRangeAsync(dateRange.StartDate, dateRange.EndDate);

        viewModel.Sessions = sessions
            .Select(session =>
            {
                var paidBookings = session.Bookings.Where(booking => booking.Status != "Cancelled").ToList();

                return new AdminRevenueSessionItemViewModel
                {
                    SessionId = session.Id,
                    SessionTime = session.SessionTime,
                    FilmTitle = session.Film.Title,
                    HallName = session.Hall.Name,
                    Technology = session.Hall.Technology,
                    TicketPrice = session.Price,
                    SoldSeatsCount = paidBookings.Sum(booking => booking.BookedSeats.Count),
                    Revenue = paidBookings.Sum(booking => booking.TotalPrice)
                };
            })
            .OrderBy(item => item.SessionTime)
            .ToList();

        viewModel.TotalRevenue = viewModel.Sessions.Sum(item => item.Revenue);
        viewModel.TotalSoldSeats = viewModel.Sessions.Sum(item => item.SoldSeatsCount);
        viewModel.TotalPaidBookings = sessions.Sum(session => session.Bookings.Count(booking => booking.Status != "Cancelled"));

        return viewModel;
    }

    private async Task<AdminReportsViewModel> BuildReportsViewModelAsync(DateTime? startDate, DateTime? endDate)
    {
        var dateRange = await ResolveReportDateRangeAsync(startDate, endDate);
        var viewModel = new AdminReportsViewModel
        {
            StartDate = dateRange.StartDate,
            EndDate = dateRange.EndDate,
            ErrorMessage = dateRange.ErrorMessage
        };

        if (!string.IsNullOrWhiteSpace(dateRange.ErrorMessage))
        {
            return viewModel;
        }

        var sessions = await LoadSessionsForReportRangeAsync(dateRange.StartDate, dateRange.EndDate);
        var paidBookings = sessions
            .SelectMany(session => session.Bookings)
            .Where(booking => booking.Status != "Cancelled")
            .ToList();

        viewModel.TotalSessions = sessions.Count;
        viewModel.TotalPaidBookings = paidBookings.Count;
        viewModel.TotalSoldSeats = paidBookings.Sum(booking => booking.BookedSeats.Count);
        viewModel.TotalRevenue = paidBookings.Sum(booking => booking.TotalPrice);

        return viewModel;
    }

    private async Task<(DateTime? StartDate, DateTime? EndDate, string? ErrorMessage)> ResolveReportDateRangeAsync(DateTime? startDate, DateTime? endDate)
    {
        var suggestedRange = await dbContext.Sessions
            .AsNoTracking()
            .GroupBy(_ => 1)
            .Select(group => new
            {
                MinDate = group.Min(item => (DateTime?)item.SessionTime),
                MaxDate = group.Max(item => (DateTime?)item.SessionTime)
            })
            .FirstOrDefaultAsync();

        var normalizedStartDate = startDate?.Date ?? suggestedRange?.MinDate?.Date;
        var normalizedEndDate = endDate?.Date ?? suggestedRange?.MaxDate?.Date;

        if (normalizedStartDate.HasValue && normalizedEndDate.HasValue && normalizedStartDate > normalizedEndDate)
        {
            return (normalizedStartDate, normalizedEndDate, "Дата початку не може бути пізнішою за дату завершення.");
        }

        return (normalizedStartDate, normalizedEndDate, null);
    }

    private async Task<List<Session>> LoadSessionsForReportRangeAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = dbContext.Sessions
            .AsNoTracking()
            .AsSplitQuery()
            .Include(item => item.Film)
            .Include(item => item.Hall)
            .Include(item => item.Bookings)
                .ThenInclude(booking => booking.BookedSeats)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(item => item.SessionTime >= startDate.Value.Date);
        }

        if (endDate.HasValue)
        {
            query = query.Where(item => item.SessionTime < endDate.Value.Date.AddDays(1));
        }

        return await query
            .OrderBy(item => item.SessionTime)
            .ToListAsync();
    }

    private byte[] BuildExcelWorkbook(IReadOnlyList<Session> sessions)
    {
        var paidBookings = sessions
            .SelectMany(session => session.Bookings)
            .Where(booking => booking.Status != "Cancelled")
            .ToList();

        var revenueSummaryRows = new List<IReadOnlyList<object?>>
        {
            new object?[]
            {
                sessions.Count,
                paidBookings.Count,
                paidBookings.Sum(booking => booking.BookedSeats.Count),
                paidBookings.Sum(booking => booking.TotalPrice),
                sessions.Count > 0 ? Math.Round(sessions.Average(session => session.Price), 2) : 0m
            }
        };

        var bookingSummaryRows = sessions
            .SelectMany(session => session.Bookings)
            .GroupBy(booking => booking.Status)
            .OrderBy(group => group.Key)
            .Select(group => (IReadOnlyList<object?>)new object?[]
            {
                TranslateBookingStatus(group.Key),
                group.Count(),
                group.Sum(booking => booking.BookedSeats.Count),
                group.Sum(booking => booking.Status == "Cancelled" ? 0m : booking.TotalPrice)
            })
            .ToList();

        var popularityRows = sessions
            .GroupBy(session => new { session.FilmId, session.Film.Title, session.Film.Genre })
            .Select(group =>
            {
                var groupedPaidBookings = group
                    .SelectMany(session => session.Bookings)
                    .Where(booking => booking.Status != "Cancelled")
                    .ToList();

                return (IReadOnlyList<object?>)new object?[]
                {
                    group.Key.Title,
                    group.Key.Genre,
                    group.Count(),
                    groupedPaidBookings.Count,
                    groupedPaidBookings.Sum(booking => booking.BookedSeats.Count)
                };
            })
            .OrderByDescending(row => Convert.ToInt32(row[4]))
            .ThenByDescending(row => Convert.ToInt32(row[3]))
            .ThenBy(row => Convert.ToString(row[0]))
            .ToList();

        var sessionsByHallRows = sessions
            .GroupBy(session => new { session.HallId, session.Hall.Name, session.Hall.Technology })
            .Select(group =>
            {
                var groupedPaidBookings = group
                    .SelectMany(session => session.Bookings)
                    .Where(booking => booking.Status != "Cancelled")
                    .ToList();

                return (IReadOnlyList<object?>)new object?[]
                {
                    group.Key.Name,
                    group.Key.Technology,
                    group.Count(),
                    groupedPaidBookings.Count,
                    groupedPaidBookings.Sum(booking => booking.BookedSeats.Count),
                    groupedPaidBookings.Sum(booking => booking.TotalPrice)
                };
            })
            .OrderBy(row => Convert.ToString(row[0]))
            .ToList();

        var workbookSheets = new List<ExcelWorksheetData>
        {
            new(
                "Огляд доходів",
                ["Всього сеансів", "Оплачених бронювань", "Продано місць", "Загальний дохід, грн", "Середня ціна квитка, грн"],
                revenueSummaryRows),
            new(
                "Огляд бронювань",
                ["Статус", "Кількість бронювань", "Продано місць", "Сума, грн"],
                bookingSummaryRows),
            new(
                "Популярність фільмів",
                ["Назва фільму", "Жанр", "Кількість сеансів", "Кількість бронювань", "Продано місць"],
                popularityRows),
            new(
                "Сеанси за залами",
                ["Зал", "Технологія", "Кількість сеансів", "Оплачених бронювань", "Продано місць", "Дохід, грн"],
                sessionsByHallRows)
        };

        return SimpleXlsxWorkbookBuilder.Build(workbookSheets);
    }

    private async Task PersistExcelReportAsync(string fileName, byte[] content)
    {
        var reportsRelativePath = configuration["Storage:ReportsPath"] ?? "App_Data/reports";
        var reportsDirectory = Path.IsPathRooted(reportsRelativePath)
            ? reportsRelativePath
            : Path.Combine(environment.ContentRootPath, reportsRelativePath);

        Directory.CreateDirectory(reportsDirectory);

        var filePath = Path.Combine(reportsDirectory, fileName);
        await System.IO.File.WriteAllBytesAsync(filePath, content);
    }

    private static string BuildSessionSummary(Session session)
    {
        return $"{session.Film.Title} • {session.SessionTime.ToString("dd.MM.yyyy HH:mm", CinemaPresentationHelper.UkrainianCulture)} • {session.Hall.Name}, {session.Hall.Technology}";
    }

    private static string TranslateBookingStatus(string status)
    {
        return status switch
        {
            "Cancelled" => "Скасовано",
            "Paid" => "Оплачено",
            _ => status
        };
    }

    private async Task<string> SavePosterAsync(IFormFile? posterFile, string currentPosterPath)
    {
        if (posterFile is null)
        {
            return string.IsNullOrWhiteSpace(currentPosterPath) ? "/source/hero-img.jpg" : currentPosterPath;
        }

        var uploadsRelativePath = configuration["Storage:UploadsPath"] ?? "wwwroot/uploads";
        var uploadsDirectory = Path.IsPathRooted(uploadsRelativePath)
            ? uploadsRelativePath
            : Path.Combine(environment.ContentRootPath, uploadsRelativePath);

        Directory.CreateDirectory(uploadsDirectory);

        var extension = Path.GetExtension(posterFile.FileName).ToLowerInvariant();
        var safeBaseName = SanitizeFileName(Path.GetFileNameWithoutExtension(posterFile.FileName));
        var fileName = $"{safeBaseName}-{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsDirectory, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await posterFile.CopyToAsync(stream);

        return $"/uploads/{fileName}";
    }

    private async Task<string?> SaveTrailerAsync(IFormFile? trailerFile, string? currentTrailerPath)
    {
        if (trailerFile is null)
        {
            return string.IsNullOrWhiteSpace(currentTrailerPath) ? null : currentTrailerPath;
        }

        var uploadsRelativePath = configuration["Storage:UploadsPath"] ?? "wwwroot/uploads";
        var uploadsDirectory = Path.IsPathRooted(uploadsRelativePath)
            ? uploadsRelativePath
            : Path.Combine(environment.ContentRootPath, uploadsRelativePath);

        Directory.CreateDirectory(uploadsDirectory);

        var extension = Path.GetExtension(trailerFile.FileName).ToLowerInvariant();
        var safeBaseName = SanitizeFileName(Path.GetFileNameWithoutExtension(trailerFile.FileName));
        var fileName = $"{safeBaseName}-{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsDirectory, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await trailerFile.CopyToAsync(stream);

        return $"/uploads/{fileName}";
    }

    private void DeleteManagedPoster(string? posterPath)
    {
        if (!IsManagedPoster(posterPath) || environment.WebRootPath is null)
        {
            return;
        }

        var relativePath = posterPath!.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(environment.WebRootPath, relativePath);
        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }

    private void DeleteManagedTrailer(string? trailerPath)
    {
        if (!IsManagedTrailer(trailerPath) || environment.WebRootPath is null)
        {
            return;
        }

        var relativePath = trailerPath!.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(environment.WebRootPath, relativePath);
        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }

    private static bool IsManagedPoster(string? posterPath)
    {
        return !string.IsNullOrWhiteSpace(posterPath)
            && posterPath.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsManagedTrailer(string? trailerPath)
    {
        return !string.IsNullOrWhiteSpace(trailerPath)
            && trailerPath.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase);
    }

    private static string SanitizeFileName(string fileName)
    {
        var sanitized = new string(fileName
            .ToLowerInvariant()
            .Select(ch => char.IsLetterOrDigit(ch) ? ch : '-')
            .ToArray())
            .Trim('-');

        return string.IsNullOrWhiteSpace(sanitized) ? "poster" : sanitized;
    }

    private static IEnumerable<Seat> BuildSeatLayout(int hallId, int rowsCount, int seatsPerRow)
    {
        for (var rowNumber = 1; rowNumber <= rowsCount; rowNumber++)
        {
            for (var seatNumber = 1; seatNumber <= seatsPerRow; seatNumber++)
            {
                yield return new Seat
                {
                    HallId = hallId,
                    RowNumber = rowNumber,
                    SeatNumber = seatNumber
                };
            }
        }
    }
}
