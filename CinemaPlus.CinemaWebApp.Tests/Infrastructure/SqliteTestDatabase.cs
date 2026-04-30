using CinemaPlus.CinemaWebApp.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Tests.Infrastructure;

public sealed class SqliteTestDatabase : IDisposable
{
    private readonly SqliteConnection connection;
    private readonly DbContextOptions<ApplicationDbContext> options;

    public SqliteTestDatabase()
    {
        connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
    }

    public ApplicationDbContext CreateContext()
    {
        return new ApplicationDbContext(options);
    }

    public void Dispose()
    {
        connection.Dispose();
    }
}
