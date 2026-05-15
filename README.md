# CinemaPlus

It is a cinema web app made on ASP.NET Core MVC with EF Core and MySQL.

In the project there is:
- movie pages
- sessions
- seat booking
- ticket pdf
- login and registration
- admin panel

What is needed:
- .NET SDK 10
- MySQL

Settings are in `appsettings.Development.json`.

Run:

```bash
dotnet build
dotnet tool run dotnet-ef database update
dotnet run
```

Tests:

```bash
dotnet test
```
