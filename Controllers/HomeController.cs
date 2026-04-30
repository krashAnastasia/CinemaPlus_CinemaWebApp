using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Controllers;

public class HomeController(ApplicationDbContext dbContext, IWebHostEnvironment environment) : Controller
{
    public async Task<IActionResult> Index()
    {
        var films = await dbContext.Films
            .AsNoTracking()
            .Include(film => film.Sessions)
            .Where(film => film.AvailabilityStatus == "NowShowing"
                && film.Sessions.Any(session => session.SessionTime <= CinemaPresentationHelper.NowShowingCutoff))
            .OrderBy(film => film.AvailabilityDate)
            .ThenBy(film => film.Title)
            .ToListAsync();

        var viewModel = new MovieCatalogueViewModel
        {
            Title = "ЗАРАЗ У КІНО",
            EmptyMessage = "Наразі немає доступних сеансів.",
            ShowHero = true,
            Films = films.Select(film => new MovieCardViewModel
            {
                Id = film.Id,
                Title = CinemaPresentationHelper.FormatMovieTitle(film),
                PosterPath = CinemaPresentationHelper.ResolvePosterPath(film, environment),
                SessionSummary = CinemaPresentationHelper.FormatSessionSummary(
                    film.Sessions.Where(session => session.SessionTime <= CinemaPresentationHelper.NowShowingCutoff),
                    " • "),
                HasSessions = film.Sessions.Any(session => session.SessionTime <= CinemaPresentationHelper.NowShowingCutoff)
            }).ToList()
        };

        return View(viewModel);
    }

    [Route("coming-soon")]
    public async Task<IActionResult> ComingSoon()
    {
        var films = await dbContext.Films
            .AsNoTracking()
            .Include(film => film.Sessions)
            .Where(film => film.AvailabilityStatus == "ComingSoon"
                && film.AvailabilityDate > CinemaPresentationHelper.ComingSoonCutoff)
            .OrderBy(film => film.AvailabilityDate)
            .ThenBy(film => film.Title)
            .ToListAsync();

        var viewModel = new MovieCatalogueViewModel
        {
            Title = "СКОРО",
            EmptyMessage = "Наразі майбутні прем'єри не додані.",
            Films = films.Select(film => new MovieCardViewModel
            {
                Id = film.Id,
                Title = CinemaPresentationHelper.FormatMovieTitle(film),
                PosterPath = CinemaPresentationHelper.ResolvePosterPath(film, environment),
                SessionSummary = $"Доступно з {film.AvailabilityDate.ToString("dd MMMM yyyy", CinemaPresentationHelper.UkrainianCulture)}",
                HasSessions = film.Sessions.Any()
            }).ToList()
        };

        return View(viewModel);
    }

    [Route("faq")]
    public IActionResult Faq()
    {
        return View();
    }

    [Route("return")]
    public IActionResult Return()
    {
        return View();
    }

    [Route("rules")]
    public IActionResult Rules()
    {
        return View();
    }

    [Route("policy")]
    public IActionResult Policy()
    {
        return View();
    }

    [Route("contacts")]
    public IActionResult Contacts()
    {
        return View();
    }
}
