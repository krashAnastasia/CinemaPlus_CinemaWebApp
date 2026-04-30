using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Controllers;

[Route("sessions")]
public class SessionsController(ApplicationDbContext dbContext, IWebHostEnvironment environment) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(string? date, int? movieId, int? hallId)
    {
        var parsedDate = ParseScheduleDate(date);

        var scheduleQuery = dbContext.Sessions
            .AsNoTracking()
            .Include(session => session.Film)
            .Include(session => session.Hall)
            .Where(session => session.SessionTime <= CinemaPresentationHelper.NowShowingCutoff);

        if (parsedDate.HasValue)
        {
            var rangeStart = parsedDate.Value.ToDateTime(TimeOnly.MinValue);
            var rangeEnd = rangeStart.AddDays(1);
            scheduleQuery = scheduleQuery.Where(session => session.SessionTime >= rangeStart && session.SessionTime < rangeEnd);
        }

        if (movieId.HasValue)
        {
            scheduleQuery = scheduleQuery.Where(session => session.FilmId == movieId.Value);
        }

        if (hallId.HasValue)
        {
            scheduleQuery = scheduleQuery.Where(session => session.HallId == hallId.Value);
        }

        var sessions = await scheduleQuery
            .OrderBy(session => session.SessionTime)
            .ThenBy(session => session.Film.Title)
            .ThenBy(session => session.Hall.Name)
            .ToListAsync();

        var movieOptions = await dbContext.Films
            .AsNoTracking()
            .Where(film => film.Sessions.Any(session => session.SessionTime <= CinemaPresentationHelper.NowShowingCutoff))
            .OrderBy(film => film.Title)
            .Select(film => new SessionFilterOptionViewModel
            {
                Id = film.Id,
                Label = CinemaPresentationHelper.FormatMovieTitle(film)
            })
            .ToListAsync();

        var hallOptions = await dbContext.Halls
            .AsNoTracking()
            .Where(hall => hall.Sessions.Any(session => session.SessionTime <= CinemaPresentationHelper.NowShowingCutoff))
            .OrderBy(hall => hall.Name)
            .Select(hall => new SessionFilterOptionViewModel
            {
                Id = hall.Id,
                Label = $"{hall.Name} ({hall.Technology})"
            })
            .ToListAsync();

        var viewModel = new SessionScheduleViewModel
        {
            SelectedDateValue = parsedDate?.ToString("yyyy-MM-dd", CinemaPresentationHelper.UkrainianCulture) ?? string.Empty,
            SelectedDateDisplayValue = parsedDate?.ToString("dd.MM.yyyy", CinemaPresentationHelper.UkrainianCulture) ?? string.Empty,
            SelectedMovieId = movieId,
            SelectedHallId = hallId,
            EmptyMessage = "На обрану комбінацію фільтрів сеансів не знайдено.",
            MovieOptions = movieOptions,
            HallOptions = hallOptions,
            Sessions = sessions.Select(session => new SessionScheduleItemViewModel
            {
                SessionId = session.Id,
                MovieId = session.FilmId,
                PosterPath = CinemaPresentationHelper.ResolvePosterPath(session.Film, environment),
                MovieTitle = CinemaPresentationHelper.FormatMovieTitle(session.Film),
                DateText = session.SessionTime.ToString("dd.MM.yyyy", CinemaPresentationHelper.UkrainianCulture),
                TimeText = session.SessionTime.ToString("H:mm", CinemaPresentationHelper.UkrainianCulture),
                HallName = session.Hall.Name,
                Technology = session.Hall.Technology,
                Price = session.Price
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpGet("{sessionId:int}/seats")]
    public async Task<IActionResult> Seats(int sessionId)
    {
        var viewModel = await BuildSeatSelectionViewModelAsync(sessionId, []);
        return viewModel is null ? NotFound() : View(viewModel);
    }

    [HttpPost("{sessionId:int}/seats")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Seats(int sessionId, SeatSelectionPostViewModel request)
    {
        var selectedSeatIds = request.SelectedSeatIds
            .Where(id => id > 0)
            .Distinct()
            .ToList();

        var sessionData = await LoadSeatSelectionDataAsync(sessionId);
        if (sessionData is null)
        {
            return NotFound();
        }

        var hallSeatIds = sessionData.HallSeats
            .Select(seat => seat.Id)
            .ToHashSet();

        var bookedSeatIds = sessionData.BookedSeatIds.ToHashSet();

        if (selectedSeatIds.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Оберіть хоча б одне місце.");
        }

        if (selectedSeatIds.Any(seatId => !hallSeatIds.Contains(seatId)))
        {
            ModelState.AddModelError(string.Empty, "Обрані місця не належать до цього залу.");
        }

        if (selectedSeatIds.Any(bookedSeatIds.Contains))
        {
            ModelState.AddModelError(string.Empty, "Деякі обрані місця вже недоступні. Оновіть вибір.");
        }

        if (!ModelState.IsValid)
        {
            var safeSelectedSeatIds = selectedSeatIds
                .Where(seatId => hallSeatIds.Contains(seatId) && !bookedSeatIds.Contains(seatId))
                .ToList();

            var viewModel = BuildSeatSelectionViewModel(sessionData, safeSelectedSeatIds);
            return View(viewModel);
        }

        return RedirectToAction("Index", "Checkout", new { sessionId, seatIds = selectedSeatIds.ToArray() });
    }

    [HttpGet("/movies/{movieId:int}/sessions")]
    public async Task<IActionResult> MovieSessions(int movieId, int? sessionId)
    {
        var matchingSessionIds = await dbContext.Sessions
            .AsNoTracking()
            .Where(item => item.FilmId == movieId && item.SessionTime <= CinemaPresentationHelper.NowShowingCutoff)
            .OrderBy(item => item.SessionTime)
            .Select(item => item.Id)
            .ToListAsync();

        if (matchingSessionIds.Count == 0)
        {
            return RedirectToAction(nameof(Index), new { movieId });
        }

        var targetSessionId = sessionId.HasValue && matchingSessionIds.Contains(sessionId.Value)
            ? sessionId.Value
            : matchingSessionIds[0];

        return RedirectToAction(nameof(Seats), new { sessionId = targetSessionId });
    }

    private async Task<SeatSelectionViewModel?> BuildSeatSelectionViewModelAsync(int sessionId, IReadOnlyCollection<int> selectedSeatIds)
    {
        var sessionData = await LoadSeatSelectionDataAsync(sessionId);
        return sessionData is null ? null : BuildSeatSelectionViewModel(sessionData, selectedSeatIds);
    }

    private async Task<SeatSelectionData?> LoadSeatSelectionDataAsync(int sessionId)
    {
        var session = await dbContext.Sessions
            .AsNoTracking()
            .AsSplitQuery()
            .Include(item => item.Film)
            .Include(item => item.Hall)
                .ThenInclude(hall => hall.Seats)
            .Include(item => item.BookedSeats)
            .FirstOrDefaultAsync(item => item.Id == sessionId && item.SessionTime <= CinemaPresentationHelper.NowShowingCutoff);

        if (session is null)
        {
            return null;
        }

        var relatedSessions = await dbContext.Sessions
            .AsNoTracking()
            .Where(item => item.FilmId == session.FilmId && item.SessionTime <= CinemaPresentationHelper.NowShowingCutoff)
            .OrderBy(item => item.SessionTime)
            .ToListAsync();

        return new SeatSelectionData(
            session,
            session.Hall.Seats
                .OrderBy(seat => seat.RowNumber)
                .ThenBy(seat => seat.SeatNumber)
                .ToList(),
            session.BookedSeats.Select(item => item.SeatId).ToList(),
            relatedSessions);
    }

    private static DateOnly? ParseScheduleDate(string? rawDate)
    {
        if (string.IsNullOrWhiteSpace(rawDate))
        {
            return null;
        }

        var supportedFormats = new[] { "yyyy-MM-dd", "dd.MM.yyyy" };

        return DateOnly.TryParseExact(
            rawDate,
            supportedFormats,
            CinemaPresentationHelper.UkrainianCulture,
            System.Globalization.DateTimeStyles.None,
            out var parsedDate)
            ? parsedDate
            : null;
    }

    private SeatSelectionViewModel BuildSeatSelectionViewModel(SeatSelectionData sessionData, IReadOnlyCollection<int> selectedSeatIds)
    {
        var bookedSeatIds = sessionData.BookedSeatIds.ToHashSet();
        var selectedSeatIdSet = selectedSeatIds
            .Where(seatId => !bookedSeatIds.Contains(seatId))
            .ToHashSet();

        var selectedSeatLabels = sessionData.HallSeats
            .Where(seat => selectedSeatIdSet.Contains(seat.Id))
            .OrderBy(seat => seat.RowNumber)
            .ThenBy(seat => seat.SeatNumber)
            .Select(seat => CinemaPresentationHelper.FormatSeatLabel(seat.RowNumber, seat.SeatNumber))
            .ToList();

        var selectedDate = DateOnly.FromDateTime(sessionData.Session.SessionTime);

        var dateChips = sessionData.RelatedSessions
            .GroupBy(item => DateOnly.FromDateTime(item.SessionTime))
            .OrderBy(group => group.Key)
            .Select(group =>
            {
                var activeSessionId = group.Any(item => item.Id == sessionData.Session.Id)
                    ? sessionData.Session.Id
                    : group.First().Id;

                return new SeatSelectionDateChipViewModel
                {
                    SessionId = activeSessionId,
                    Label = group.Key.ToString("dd.MM", CinemaPresentationHelper.UkrainianCulture),
                    IsActive = group.Key == selectedDate
                };
            })
            .ToList();

        var sessionChips = sessionData.RelatedSessions
            .Where(item => DateOnly.FromDateTime(item.SessionTime) == selectedDate)
            .Select(item => new SeatSelectionSessionChipViewModel
            {
                SessionId = item.Id,
                Label = item.SessionTime.ToString("H:mm", CinemaPresentationHelper.UkrainianCulture),
                IsActive = item.Id == sessionData.Session.Id
            })
            .ToList();

        var rows = sessionData.HallSeats
            .GroupBy(seat => seat.RowNumber)
            .OrderBy(group => group.Key)
            .Select(group => new SeatSelectionRowViewModel
            {
                Label = CinemaPresentationHelper.GetRowLabel(group.Key),
                Seats = group
                    .OrderBy(seat => seat.SeatNumber)
                    .Select(seat =>
                    {
                        var isBooked = bookedSeatIds.Contains(seat.Id);
                        var isSelected = !isBooked && selectedSeatIdSet.Contains(seat.Id);

                        return new SeatSelectionSeatViewModel
                        {
                            Id = seat.Id,
                            RowLabel = CinemaPresentationHelper.GetRowLabel(seat.RowNumber),
                            SeatNumber = seat.SeatNumber,
                            IsBooked = isBooked,
                            IsSelected = isSelected,
                            AriaLabel = CinemaPresentationHelper.FormatSeatLabel(seat.RowNumber, seat.SeatNumber),
                            CssClass = isBooked
                                ? "seat seat--unavailable"
                                : isSelected
                                    ? "seat seat--selected"
                                    : "seat seat--available"
                        };
                    })
                    .ToList()
            })
            .ToList();

        return new SeatSelectionViewModel
        {
            SessionId = sessionData.Session.Id,
            MovieId = sessionData.Session.FilmId,
            MovieTitle = CinemaPresentationHelper.FormatMovieTitle(sessionData.Session.Film),
            PosterPath = CinemaPresentationHelper.ResolvePosterPath(sessionData.Session.Film, environment),
            Genre = sessionData.Session.Film.Genre,
            DurationMinutes = sessionData.Session.Film.DurationMinutes,
            AgeRestriction = sessionData.Session.Film.AgeRestriction,
            DateText = sessionData.Session.SessionTime.ToString("dd MMMM yyyy", CinemaPresentationHelper.UkrainianCulture),
            TimeText = sessionData.Session.SessionTime.ToString("H:mm", CinemaPresentationHelper.UkrainianCulture),
            HallName = sessionData.Session.Hall.Name,
            Technology = sessionData.Session.Hall.Technology,
            Price = sessionData.Session.Price,
            SeatsPerRow = sessionData.Session.Hall.SeatsPerRow,
            SessionSummaryText = CinemaPresentationHelper.FormatSessionSummary(sessionData.RelatedSessions),
            DateChips = dateChips,
            SessionChips = sessionChips,
            Rows = rows,
            SelectedSeatIds = selectedSeatIdSet.ToList(),
            SelectedSeatLabels = selectedSeatLabels,
            SelectedTotalPrice = sessionData.Session.Price * selectedSeatLabels.Count
        };
    }

    private sealed record SeatSelectionData(
        Models.Session Session,
        IReadOnlyList<Models.Seat> HallSeats,
        IReadOnlyList<int> BookedSeatIds,
        IReadOnlyList<Models.Session> RelatedSessions);
}
