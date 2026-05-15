using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);
const long maxUploadSizeBytes = 512L * 1024L * 1024L;

builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = maxUploadSizeBytes;
    options.MultipartHeadersLengthLimit = 32 * 1024;
});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = maxUploadSizeBytes;
});
builder.Services.AddScoped<IPasswordService, Pbkdf2PasswordService>();
builder.Services.AddScoped<ICinemaTicketService, CinemaTicketService>();
builder.Services.AddScoped<IBonusService, BonusService>();
builder.Services.AddSingleton<IPasswordResetService, MemoryCachePasswordResetService>();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = maxUploadSizeBytes;
});
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var databaseProvider = configuration["DatabaseProvider"] ?? "MySql";
    var connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? "server=localhost;port=3306;database=cinemaplus;user=cinemaplus_user;password=CHANGE_ME";

    if (string.Equals(databaseProvider, "Sqlite", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlite(connectionString);
        return;
    }

    options.UseMySQL(connectionString);
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login";
        options.AccessDeniedPath = "/account/access-denied";
        options.Cookie.Name = "CinemaPlus.Auth";
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program
{
}
