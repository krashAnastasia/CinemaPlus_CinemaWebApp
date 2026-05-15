namespace CinemaPlus.CinemaWebApp.Models;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? ProfilePhotoPath { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Client";

    public DateTime CreatedAt { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
