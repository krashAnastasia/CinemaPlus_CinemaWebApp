using System.Security.Claims;
using CinemaPlus.CinemaWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.FileProviders;

namespace CinemaPlus.CinemaWebApp.Tests.Infrastructure;

public static class TestControllerFactory
{
    public static void AttachContext(Controller controller, IEnumerable<Claim>? claims = null)
    {
        var httpContext = new DefaultHttpContext();

        if (claims is not null)
        {
            var identity = new ClaimsIdentity(claims, "TestAuth");
            httpContext.User = new ClaimsPrincipal(identity);
        }

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        controller.TempData = new TempDataDictionary(httpContext, new TestTempDataProvider());
    }

    public static IWebHostEnvironment CreateEnvironment()
    {
        return new TestWebHostEnvironment();
    }
}

internal sealed class TestTempDataProvider : ITempDataProvider
{
    public IDictionary<string, object> LoadTempData(HttpContext context)
    {
        return new Dictionary<string, object>();
    }

    public void SaveTempData(HttpContext context, IDictionary<string, object> values)
    {
    }
}

internal sealed class TestWebHostEnvironment : IWebHostEnvironment
{
    public string ApplicationName { get; set; } = "CinemaPlus.CinemaWebApp.Tests";

    public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();

    public string ContentRootPath { get; set; } = AppContext.BaseDirectory;

    public string EnvironmentName { get; set; } = "Development";

    public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();

    public string WebRootPath { get; set; } = string.Empty;
}

public sealed class FixedTicketService : ICinemaTicketService
{
    private readonly string ticketCode;

    public FixedTicketService(string ticketCode = "CP-UNIT-0001")
    {
        this.ticketCode = ticketCode;
    }

    public string BuildQrPayload(CinemaPlus.CinemaWebApp.ViewModels.TicketConfirmationViewModel ticket)
    {
        return $"QR:{ticket.OrderNumber}:{ticket.TicketCode}";
    }

    public string FormatOrderNumber(int bookingId, DateTime bookingDate)
    {
        return $"{bookingDate:yyMMdd}{bookingId:D4}";
    }

    public string GenerateQrCodeDataUri(string payload)
    {
        return $"data:text/plain;base64,{Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(payload))}";
    }

    public byte[] GenerateTicketPdf(CinemaPlus.CinemaWebApp.ViewModels.TicketConfirmationViewModel ticket)
    {
        return System.Text.Encoding.UTF8.GetBytes($"PDF:{ticket.OrderNumber}:{ticket.TicketCode}");
    }

    public string GenerateTicketCode()
    {
        return ticketCode;
    }
}
