using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Controllers;

[Route("movies")]
public class MoviesController(ApplicationDbContext dbContext, IWebHostEnvironment environment) : Controller
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, int? sessionId)
    {
        var film = await dbContext.Films
            .AsNoTracking()
            .Include(item => item.Sessions)
                .ThenInclude(session => session.Hall)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (film is null)
        {
            return NotFound();
        }

        var availableSessions = film.Sessions
            .Where(session => session.SessionTime <= CinemaPresentationHelper.NowShowingCutoff)
            .OrderBy(session => session.SessionTime)
            .ToList();

        var sessionViewModels = availableSessions
            .Select(session => new MovieSessionViewModel
            {
                Id = session.Id,
                Date = DateOnly.FromDateTime(session.SessionTime),
                DateText = session.SessionTime.ToString("dd MMMM yyyy", CinemaPresentationHelper.UkrainianCulture),
                TimeText = session.SessionTime.ToString("H:mm", CinemaPresentationHelper.UkrainianCulture),
                HallName = session.Hall.Name,
                Technology = session.Hall.Technology,
                Price = session.Price
            })
            .ToList();

        var primarySession = sessionViewModels.FirstOrDefault();
        var selectedSession = sessionViewModels
            .FirstOrDefault(session => sessionId.HasValue && session.Id == sessionId.Value)
            ?? primarySession;

        var selectedDate = selectedSession?.Date;

        var dateChips = sessionViewModels
            .GroupBy(session => session.Date)
            .OrderBy(group => group.Key)
            .Select(group =>
            {
                var targetSession = selectedSession is not null && group.Any(session => session.Id == selectedSession.Id)
                    ? selectedSession
                    : group.First();

                return new MovieDetailsDateChipViewModel
                {
                    SessionId = targetSession.Id,
                    Label = group.Key.ToString("dd.MM", CinemaPresentationHelper.UkrainianCulture),
                    IsActive = selectedDate.HasValue && group.Key == selectedDate.Value
                };
            })
            .ToList();

        var visibleSessions = selectedDate.HasValue
            ? sessionViewModels.Where(session => session.Date == selectedDate.Value)
            : sessionViewModels;

        var sessionChips = visibleSessions
            .Select(session => new MovieDetailsSessionChipViewModel
            {
                SessionId = session.Id,
                Label = session.TimeText,
                HallLabel = $"{session.HallName}, {session.Technology}",
                PriceLabel = $"{session.Price:0} грн",
                IsActive = selectedSession is not null && session.Id == selectedSession.Id
            })
            .ToList();

        var viewModel = new MovieDetailsViewModel
        {
            Id = film.Id,
            Title = CinemaPresentationHelper.FormatMovieTitle(film),
            Genre = film.Genre,
            DurationMinutes = film.DurationMinutes,
            Description = film.Description,
            ReleaseYear = film.ReleaseYear,
            AgeRestriction = film.AgeRestriction,
            PosterPath = CinemaPresentationHelper.ResolvePosterPath(film, environment),
            TrailerPath = film.TrailerPath ?? string.Empty,
            HasTrailer = !string.IsNullOrWhiteSpace(film.TrailerPath),
            AvailabilityText = film.AvailabilityStatus == "ComingSoon"
                ? $"СКОРО З {film.AvailabilityDate.ToString("dd MMMM yyyy", CinemaPresentationHelper.UkrainianCulture).ToUpper(CinemaPresentationHelper.UkrainianCulture)}"
                : "У КІНО",
            SessionSummary = CinemaPresentationHelper.FormatSessionSummary(availableSessions),
            HasAvailableSessions = availableSessions.Count > 0,
            PrimarySessionId = primarySession?.Id,
            SelectedSessionId = selectedSession?.Id,
            SelectedSessionDateLabel = selectedSession?.DateText ?? string.Empty,
            SelectedSessionHallLabel = selectedSession is not null
                ? $"{selectedSession.HallName}, {selectedSession.Technology}"
                : string.Empty,
            SelectedSessionPriceLabel = selectedSession is not null
                ? $"{selectedSession.Price:0} грн"
                : string.Empty,
            DateChips = dateChips,
            SessionChips = sessionChips,
            Sessions = sessionViewModels
        };

        return View(viewModel);
    }
}
