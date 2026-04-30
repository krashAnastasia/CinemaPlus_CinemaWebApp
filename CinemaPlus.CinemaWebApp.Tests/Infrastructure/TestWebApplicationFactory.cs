using CinemaPlus.CinemaWebApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaPlus.CinemaWebApp.Tests.Infrastructure;

public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncDisposable
{
    private readonly string rootDirectory = Path.Combine(Path.GetTempPath(), "CinemaPlusTests", Guid.NewGuid().ToString("N"));
    private string? databasePath;
    private string? uploadsPath;
    private string? reportsPath;

    public ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite($"Data Source={databasePath}")
            .EnableSensitiveDataLogging()
            .Options;

        return new ApplicationDbContext(options);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Directory.CreateDirectory(rootDirectory);
        databasePath = Path.Combine(rootDirectory, "cinemaplus-tests.sqlite");
        uploadsPath = Path.Combine(rootDirectory, "uploads");
        reportsPath = Path.Combine(rootDirectory, "reports");
        Directory.CreateDirectory(uploadsPath);
        Directory.CreateDirectory(reportsPath);

        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DatabaseProvider"] = "Sqlite",
                ["ConnectionStrings:DefaultConnection"] = $"Data Source={databasePath}",
                ["Storage:UploadsPath"] = uploadsPath,
                ["Storage:ReportsPath"] = reportsPath
            });
        });

        builder.ConfigureServices(services =>
        {
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        });
    }

    public new async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();

        if (Directory.Exists(rootDirectory))
        {
            Directory.Delete(rootDirectory, recursive: true);
        }
    }
}
