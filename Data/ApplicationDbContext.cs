using CinemaPlus.CinemaWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Film> Films => Set<Film>();

    public DbSet<Hall> Halls => Set<Hall>();

    public DbSet<Seat> Seats => Set<Seat>();

    public DbSet<Session> Sessions => Set<Session>();

    public DbSet<Booking> Bookings => Set<Booking>();

    public DbSet<BookedSeat> BookedSeats => Set<BookedSeat>();

    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUsers(modelBuilder);
        ConfigureFilms(modelBuilder);
        ConfigureHalls(modelBuilder);
        ConfigureSeats(modelBuilder);
        ConfigureSessions(modelBuilder);
        ConfigureBookings(modelBuilder);
        ConfigureBookedSeats(modelBuilder);
        ConfigureNotificationLogs(modelBuilder);
        SeedDemoData(modelBuilder);
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.FullName).HasMaxLength(150).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(180).IsRequired();
            entity.Property(user => user.Phone).HasMaxLength(30);
            entity.Property(user => user.PasswordHash).HasMaxLength(255).IsRequired();
            entity.Property(user => user.Role).HasMaxLength(30).IsRequired();
            entity.Property(user => user.CreatedAt).IsRequired();
            entity.HasIndex(user => user.Email).IsUnique();
        });
    }

    private static void ConfigureFilms(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Film>(entity =>
        {
            entity.ToTable("Films");
            entity.HasKey(film => film.Id);
            entity.Property(film => film.Title).HasMaxLength(200).IsRequired();
            entity.Property(film => film.Genre).HasMaxLength(100).IsRequired();
            entity.Property(film => film.Description).HasColumnType("text").IsRequired();
            entity.Property(film => film.AgeRestriction).HasMaxLength(20).IsRequired();
            entity.Property(film => film.PosterPath).HasMaxLength(300).IsRequired();
            entity.Property(film => film.TrailerPath).HasMaxLength(300);
            entity.Property(film => film.AvailabilityDate)
                .HasConversion(
                    date => date.ToDateTime(TimeOnly.MinValue),
                    dateTime => DateOnly.FromDateTime(dateTime))
                .HasColumnType("date")
                .IsRequired();
            entity.Property(film => film.AvailabilityStatus).HasMaxLength(40).IsRequired();
            entity.HasIndex(film => film.Title).IsUnique();
            entity.HasIndex(film => new { film.AvailabilityStatus, film.AvailabilityDate });
        });
    }

    private static void ConfigureHalls(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hall>(entity =>
        {
            entity.ToTable("Halls");
            entity.HasKey(hall => hall.Id);
            entity.Property(hall => hall.Name).HasMaxLength(80).IsRequired();
            entity.Property(hall => hall.Technology).HasMaxLength(80).IsRequired();
            entity.HasIndex(hall => hall.Name).IsUnique();
        });
    }

    private static void ConfigureSeats(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Seat>(entity =>
        {
            entity.ToTable("Seats");
            entity.HasKey(seat => seat.Id);
            entity.HasOne(seat => seat.Hall)
                .WithMany(hall => hall.Seats)
                .HasForeignKey(seat => seat.HallId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(seat => new { seat.HallId, seat.RowNumber, seat.SeatNumber }).IsUnique();
        });
    }

    private static void ConfigureSessions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Session>(entity =>
        {
            entity.ToTable("Sessions");
            entity.HasKey(session => session.Id);
            entity.Property(session => session.Price).HasPrecision(10, 2).IsRequired();
            entity.HasOne(session => session.Film)
                .WithMany(film => film.Sessions)
                .HasForeignKey(session => session.FilmId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(session => session.Hall)
                .WithMany(hall => hall.Sessions)
                .HasForeignKey(session => session.HallId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(session => new { session.FilmId, session.HallId, session.SessionTime }).IsUnique();
            entity.HasIndex(session => session.SessionTime);
        });
    }

    private static void ConfigureBookings(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Bookings");
            entity.HasKey(booking => booking.Id);
            entity.Property(booking => booking.Status).HasMaxLength(40).IsRequired();
            entity.Property(booking => booking.TotalPrice).HasPrecision(10, 2).IsRequired();
            entity.Property(booking => booking.TicketCode).HasMaxLength(80).IsRequired();
            entity.Property(booking => booking.CustomerName).HasMaxLength(150).IsRequired();
            entity.Property(booking => booking.CustomerEmail).HasMaxLength(180).IsRequired();
            entity.Property(booking => booking.CustomerPhone).HasMaxLength(30);
            entity.HasOne(booking => booking.User)
                .WithMany(user => user.Bookings)
                .HasForeignKey(booking => booking.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(booking => booking.Session)
                .WithMany(session => session.Bookings)
                .HasForeignKey(booking => booking.SessionId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(booking => booking.TicketCode).IsUnique();
            entity.HasIndex(booking => booking.BookingDate);
        });
    }

    private static void ConfigureBookedSeats(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookedSeat>(entity =>
        {
            entity.ToTable("BookedSeats");
            entity.HasKey(bookedSeat => bookedSeat.Id);
            entity.HasOne(bookedSeat => bookedSeat.Booking)
                .WithMany(booking => booking.BookedSeats)
                .HasForeignKey(bookedSeat => bookedSeat.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(bookedSeat => bookedSeat.Session)
                .WithMany(session => session.BookedSeats)
                .HasForeignKey(bookedSeat => bookedSeat.SessionId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(bookedSeat => bookedSeat.Seat)
                .WithMany(seat => seat.BookedSeats)
                .HasForeignKey(bookedSeat => bookedSeat.SeatId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(bookedSeat => new { bookedSeat.SessionId, bookedSeat.SeatId }).IsUnique();
            entity.HasIndex(bookedSeat => new { bookedSeat.BookingId, bookedSeat.SeatId }).IsUnique();
        });
    }

    private static void ConfigureNotificationLogs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationLog>(entity =>
        {
            entity.ToTable("NotificationLogs");
            entity.HasKey(log => log.Id);
            entity.Property(log => log.Email).HasMaxLength(180).IsRequired();
            entity.Property(log => log.Message).HasColumnType("text").IsRequired();
            entity.Property(log => log.Status).HasMaxLength(40).IsRequired();
            entity.HasOne(log => log.Booking)
                .WithMany(booking => booking.NotificationLogs)
                .HasForeignKey(log => log.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void SeedDemoData(ModelBuilder modelBuilder)
    {
        var createdAt = new DateTime(2026, 4, 29, 12, 0, 0);
        const string sharedTrailerPath = "/uploads/bentogalleryintroduction-ed00fda634c040c393442749fd463d8d.mov";

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FullName = "Адміністратор CinemaPlus",
                Email = "admin@cinemaplus.local",
                Phone = "+380501112233",
                PasswordHash = "PBKDF2-SHA256$100000$Q2luZW1hUGx1c0FkbWluU2VlZFNhbHQyMDI2$QH4wb86t1iJR1VcbnplnhpgMk7Q4ROIyNmqOM6m5+Sg=",
                Role = "Admin",
                CreatedAt = createdAt
            },
            new User
            {
                Id = 2,
                FullName = "Олена Коваль",
                Email = "client@cinemaplus.local",
                Phone = "+380672223344",
                PasswordHash = "PBKDF2-SHA256$100000$Q2luZW1hUGx1c0NsaWVudFNlZWRTYWx0MjAyNg==$Np9+ovw8byxBN8DeP7YEN+iDXUE+4HGEA0+05IxctiE=",
                Role = "Client",
                CreatedAt = createdAt
            });

        modelBuilder.Entity<Hall>().HasData(
            new Hall { Id = 1, Name = "Зал 1", Technology = "2D", RowsCount = 6, SeatsPerRow = 10 },
            new Hall { Id = 2, Name = "Зал 2", Technology = "3D", RowsCount = 8, SeatsPerRow = 12 },
            new Hall { Id = 3, Name = "VIP зал", Technology = "IMAX", RowsCount = 5, SeatsPerRow = 8 });

        modelBuilder.Entity<Seat>().HasData(BuildSeatSeed());

        modelBuilder.Entity<Film>().HasData(BuildFilmSeed(sharedTrailerPath));
        modelBuilder.Entity<Session>().HasData(BuildSessionSeed());

        modelBuilder.Entity<Booking>().HasData(
            new Booking
            {
                Id = 1,
                UserId = 2,
                SessionId = 1,
                BookingDate = new DateTime(2026, 5, 1, 10, 15, 0),
                Status = "Paid",
                TotalPrice = 360m,
                TicketCode = "CP-20260501-0001",
                CustomerName = "Олена Коваль",
                CustomerEmail = "client@cinemaplus.local",
                CustomerPhone = "+380672223344"
            },
            new Booking
            {
                Id = 2,
                UserId = 2,
                SessionId = 5,
                BookingDate = new DateTime(2026, 5, 2, 14, 05, 0),
                Status = "Paid",
                TotalPrice = 360m,
                TicketCode = "CP-20260502-0002",
                CustomerName = "Олена Коваль",
                CustomerEmail = "client@cinemaplus.local",
                CustomerPhone = "+380672223344"
            },
            new Booking
            {
                Id = 3,
                UserId = null,
                SessionId = 7,
                BookingDate = new DateTime(2026, 5, 3, 18, 20, 0),
                Status = "Paid",
                TotalPrice = 660m,
                TicketCode = "CP-20260503-0003",
                CustomerName = "Марина Стеценко",
                CustomerEmail = "marina.demo@cinemaplus.local",
                CustomerPhone = "+380931110022"
            },
            new Booking
            {
                Id = 4,
                UserId = null,
                SessionId = 20,
                BookingDate = new DateTime(2026, 5, 4, 11, 40, 0),
                Status = "Paid",
                TotalPrice = 520m,
                TicketCode = "CP-20260504-0004",
                CustomerName = "Ігор Мельник",
                CustomerEmail = "igor.demo@cinemaplus.local",
                CustomerPhone = "+380661230045"
            },
            new Booking
            {
                Id = 5,
                UserId = null,
                SessionId = 3,
                BookingDate = new DateTime(2026, 5, 5, 12, 05, 0),
                Status = "Paid",
                TotalPrice = 880m,
                TicketCode = "CP-20260505-0005",
                CustomerName = "Наталія Гринь",
                CustomerEmail = "nataliia.demo@cinemaplus.local",
                CustomerPhone = "+380971230056"
            },
            new Booking
            {
                Id = 6,
                UserId = 2,
                SessionId = 8,
                BookingDate = new DateTime(2026, 5, 5, 16, 35, 0),
                Status = "Paid",
                TotalPrice = 660m,
                TicketCode = "CP-20260505-0006",
                CustomerName = "Олена Коваль",
                CustomerEmail = "client@cinemaplus.local",
                CustomerPhone = "+380672223344"
            },
            new Booking
            {
                Id = 7,
                UserId = null,
                SessionId = 16,
                BookingDate = new DateTime(2026, 5, 6, 9, 10, 0),
                Status = "Paid",
                TotalPrice = 510m,
                TicketCode = "CP-20260506-0007",
                CustomerName = "Андрій Савчук",
                CustomerEmail = "andrii.demo@cinemaplus.local",
                CustomerPhone = "+380501450067"
            },
            new Booking
            {
                Id = 8,
                UserId = null,
                SessionId = 23,
                BookingDate = new DateTime(2026, 5, 6, 19, 05, 0),
                Status = "Paid",
                TotalPrice = 840m,
                TicketCode = "CP-20260506-0008",
                CustomerName = "Юлія Бондар",
                CustomerEmail = "yuliia.demo@cinemaplus.local",
                CustomerPhone = "+380631220078"
            });

        modelBuilder.Entity<BookedSeat>().HasData(
            new BookedSeat { Id = 1, BookingId = 1, SessionId = 1, SeatId = 25 },
            new BookedSeat { Id = 2, BookingId = 1, SessionId = 1, SeatId = 26 },
            new BookedSeat { Id = 3, BookingId = 2, SessionId = 5, SeatId = 14 },
            new BookedSeat { Id = 4, BookingId = 2, SessionId = 5, SeatId = 15 },
            new BookedSeat { Id = 5, BookingId = 3, SessionId = 7, SeatId = 73 },
            new BookedSeat { Id = 6, BookingId = 3, SessionId = 7, SeatId = 74 },
            new BookedSeat { Id = 7, BookingId = 3, SessionId = 7, SeatId = 75 },
            new BookedSeat { Id = 8, BookingId = 4, SessionId = 20, SeatId = 173 },
            new BookedSeat { Id = 9, BookingId = 4, SessionId = 20, SeatId = 174 },
            new BookedSeat { Id = 10, BookingId = 5, SessionId = 3, SeatId = 86 },
            new BookedSeat { Id = 11, BookingId = 5, SessionId = 3, SeatId = 87 },
            new BookedSeat { Id = 12, BookingId = 5, SessionId = 3, SeatId = 98 },
            new BookedSeat { Id = 13, BookingId = 5, SessionId = 3, SeatId = 99 },
            new BookedSeat { Id = 14, BookingId = 6, SessionId = 8, SeatId = 109 },
            new BookedSeat { Id = 15, BookingId = 6, SessionId = 8, SeatId = 110 },
            new BookedSeat { Id = 16, BookingId = 6, SessionId = 8, SeatId = 121 },
            new BookedSeat { Id = 17, BookingId = 7, SessionId = 16, SeatId = 31 },
            new BookedSeat { Id = 18, BookingId = 7, SessionId = 16, SeatId = 32 },
            new BookedSeat { Id = 19, BookingId = 7, SessionId = 16, SeatId = 33 },
            new BookedSeat { Id = 20, BookingId = 8, SessionId = 23, SeatId = 189 },
            new BookedSeat { Id = 21, BookingId = 8, SessionId = 23, SeatId = 190 },
            new BookedSeat { Id = 22, BookingId = 8, SessionId = 23, SeatId = 195 });

        modelBuilder.Entity<NotificationLog>().HasData(
            new NotificationLog
            {
                Id = 1,
                BookingId = 1,
                Email = "client@cinemaplus.local",
                Message = "Підтвердження бронювання CP-20260501-0001",
                CreatedDate = new DateTime(2026, 5, 1, 10, 16, 0),
                Status = "Emulated"
            },
            new NotificationLog
            {
                Id = 2,
                BookingId = 2,
                Email = "client@cinemaplus.local",
                Message = "Підтвердження бронювання CP-20260502-0002",
                CreatedDate = new DateTime(2026, 5, 2, 14, 06, 0),
                Status = "Emulated"
            },
            new NotificationLog
            {
                Id = 3,
                BookingId = 3,
                Email = "marina.demo@cinemaplus.local",
                Message = "Підтвердження бронювання CP-20260503-0003",
                CreatedDate = new DateTime(2026, 5, 3, 18, 21, 0),
                Status = "Emulated"
            },
            new NotificationLog
            {
                Id = 4,
                BookingId = 4,
                Email = "igor.demo@cinemaplus.local",
                Message = "Підтвердження бронювання CP-20260504-0004",
                CreatedDate = new DateTime(2026, 5, 4, 11, 41, 0),
                Status = "Emulated"
            },
            new NotificationLog
            {
                Id = 5,
                BookingId = 5,
                Email = "nataliia.demo@cinemaplus.local",
                Message = "Підтвердження бронювання CP-20260505-0005",
                CreatedDate = new DateTime(2026, 5, 5, 12, 06, 0),
                Status = "Emulated"
            },
            new NotificationLog
            {
                Id = 6,
                BookingId = 6,
                Email = "client@cinemaplus.local",
                Message = "Підтвердження бронювання CP-20260505-0006",
                CreatedDate = new DateTime(2026, 5, 5, 16, 36, 0),
                Status = "Emulated"
            },
            new NotificationLog
            {
                Id = 7,
                BookingId = 7,
                Email = "andrii.demo@cinemaplus.local",
                Message = "Підтвердження бронювання CP-20260506-0007",
                CreatedDate = new DateTime(2026, 5, 6, 9, 11, 0),
                Status = "Emulated"
            },
            new NotificationLog
            {
                Id = 8,
                BookingId = 8,
                Email = "yuliia.demo@cinemaplus.local",
                Message = "Підтвердження бронювання CP-20260506-0008",
                CreatedDate = new DateTime(2026, 5, 6, 19, 06, 0),
                Status = "Emulated"
            });
    }

    private static IEnumerable<Seat> BuildSeatSeed()
    {
        var seats = new List<Seat>();
        var id = 1;

        AddHallSeats(hallId: 1, rowsCount: 6, seatsPerRow: 10);
        AddHallSeats(hallId: 2, rowsCount: 8, seatsPerRow: 12);
        AddHallSeats(hallId: 3, rowsCount: 5, seatsPerRow: 8);

        return seats;

        void AddHallSeats(int hallId, int rowsCount, int seatsPerRow)
        {
            for (var row = 1; row <= rowsCount; row++)
            {
                for (var seatNumber = 1; seatNumber <= seatsPerRow; seatNumber++)
                {
                    seats.Add(new Seat
                    {
                        Id = id++,
                        HallId = hallId,
                        RowNumber = row,
                        SeatNumber = seatNumber
                    });
                }
            }
        }
    }

    private static IEnumerable<Film> BuildFilmSeed(string sharedTrailerPath)
    {
        return
        [
            new Film
            {
                Id = 1,
                Title = "Аватар: Вогонь і попіл (2025)",
                Genre = "Фантастика",
                DurationMinutes = 190,
                Description = "Нова подорож Пандорою, де родина Саллі стикається з вогняним народом та небезпекою, що змінює майбутнє На'ві.",
                ReleaseYear = 2025,
                AgeRestriction = "12+",
                PosterPath = "/source/avatar-poster.webp",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 7, 1),
                AvailabilityStatus = "NowShowing"
            },
            new Film
            {
                Id = 2,
                Title = "Дюна: Частина третя (2026)",
                Genre = "Фантастика",
                DurationMinutes = 160,
                Description = "Боротьба за Арракіс виходить за межі однієї планети, а Пол Атрейдес робить вибір, що змінить майбутнє Імперіуму.",
                ReleaseYear = 2026,
                AgeRestriction = "12+",
                PosterPath = "/source/images.jpeg",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 7, 2),
                AvailabilityStatus = "NowShowing"
            },
            new Film
            {
                Id = 3,
                Title = "Воно",
                Genre = "Жахи",
                DurationMinutes = 135,
                Description = "Друзі з маленького міста зустрічаються з давнім страхом, який повертається у найтемніших образах.",
                ReleaseYear = 2017,
                AgeRestriction = "16+",
                PosterPath = "/source/it-poster.jpg",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 7, 3),
                AvailabilityStatus = "NowShowing"
            },
            new Film
            {
                Id = 4,
                Title = "Матриця",
                Genre = "Фантастика",
                DurationMinutes = 136,
                Description = "Нео відкриває правду про світ, у якому живе, та вступає у боротьбу з системою Матриці.",
                ReleaseYear = 1999,
                AgeRestriction = "16+",
                PosterPath = "/source/matrix-poster.webp",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 7, 4),
                AvailabilityStatus = "NowShowing"
            },
            new Film
            {
                Id = 5,
                Title = "Дивні дива: Фінал (2025)",
                Genre = "Фантастика",
                DurationMinutes = 120,
                Description = "Остання битва друзів з Гокінса проти темряви, що загрожує їхньому місту та всьому світу.",
                ReleaseYear = 2025,
                AgeRestriction = "16+",
                PosterPath = "/source/str-things-poster.jpg",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 7, 5),
                AvailabilityStatus = "NowShowing"
            },
            new Film
            {
                Id = 6,
                Title = "Месники",
                Genre = "Екшн",
                DurationMinutes = 143,
                Description = "Команда супергероїв об'єднується, щоб зупинити загрозу, з якою неможливо впоратися поодинці.",
                ReleaseYear = 2012,
                AgeRestriction = "12+",
                PosterPath = "/source/avengers-poster.jpg",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 7, 6),
                AvailabilityStatus = "NowShowing"
            },
            new Film
            {
                Id = 7,
                Title = "Місія: Сонячний рубіж (2026)",
                Genre = "Пригоди",
                DurationMinutes = 118,
                Description = "Екіпаж вирушає до межі Сонячної системи, де на них чекає відкриття, здатне змінити історію людства.",
                ReleaseYear = 2026,
                AgeRestriction = "12+",
                PosterPath = "/source/hero-img.jpg",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 9, 15),
                AvailabilityStatus = "ComingSoon"
            },
            new Film
            {
                Id = 8,
                Title = "Лісова легенда (2026)",
                Genre = "Анімація",
                DurationMinutes = 104,
                Description = "Сімейна пригода про магічний ліс, дружбу та силу сміливого вибору.",
                ReleaseYear = 2026,
                AgeRestriction = "0+",
                PosterPath = "/source/form-hero.png",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 10, 20),
                AvailabilityStatus = "ComingSoon"
            },
            new Film
            {
                Id = 9,
                Title = "Людина-павук 3",
                Genre = "Екшн",
                DurationMinutes = 139,
                Description = "Пітер Паркер намагається втримати баланс між героїзмом, почуттями та новою темною силою, що загрожує місту.",
                ReleaseYear = 2007,
                AgeRestriction = "12+",
                PosterPath = "/source/avengers-poster.jpg",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 7, 7),
                AvailabilityStatus = "NowShowing"
            },
            new Film
            {
                Id = 10,
                Title = "Світ Юрського періоду",
                Genre = "Пригоди",
                DurationMinutes = 124,
                Description = "Нова ера парку динозаврів виходить з-під контролю, і відвідувачам доводиться боротися за виживання.",
                ReleaseYear = 2015,
                AgeRestriction = "12+",
                PosterPath = "/source/hero-img.jpg",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 7, 8),
                AvailabilityStatus = "NowShowing"
            },
            new Film
            {
                Id = 11,
                Title = "Сутінки",
                Genre = "Романтика",
                DurationMinutes = 122,
                Description = "Белла Свон переїжджає до Форкса та знайомиться з Едвардом Калленом, що змінює її життя назавжди.",
                ReleaseYear = 2008,
                AgeRestriction = "12+",
                PosterPath = "/source/form-hero.png",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 7, 9),
                AvailabilityStatus = "NowShowing"
            },
            new Film
            {
                Id = 12,
                Title = "Зоотрополіс 2",
                Genre = "Анімація",
                DurationMinutes = 108,
                Description = "Джуді Гопс і Нік Вайлд повертаються з новою справою, яка випробує їхню дружбу та спритність.",
                ReleaseYear = 2025,
                AgeRestriction = "0+",
                PosterPath = "/source/logo-blue.png",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 7, 10),
                AvailabilityStatus = "NowShowing"
            },
            new Film
            {
                Id = 13,
                Title = "Бетмен (2024)",
                Genre = "Екшн",
                DurationMinutes = 176,
                Description = "Темний лицар повертається до Ґотема, щоб розслідувати серію злочинів і зупинити нову хвилю хаосу.",
                ReleaseYear = 2024,
                AgeRestriction = "16+",
                PosterPath = "/source/cinema-logo.png",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 7, 11),
                AvailabilityStatus = "NowShowing"
            },
            new Film
            {
                Id = 14,
                Title = "Форрест Ґамп",
                Genre = "Драма",
                DurationMinutes = 142,
                Description = "Історія доброго та щирого Форреста, чий незвичайний життєвий шлях проходить крізь найяскравіші події епохи.",
                ReleaseYear = 1994,
                AgeRestriction = "12+",
                PosterPath = "/source/hero-img.jpg",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 9, 22),
                AvailabilityStatus = "ComingSoon"
            },
            new Film
            {
                Id = 15,
                Title = "Інтерстеллар",
                Genre = "Фантастика",
                DurationMinutes = 169,
                Description = "Група дослідників вирушає крізь червоточину, щоб знайти людству новий шанс на життя.",
                ReleaseYear = 2014,
                AgeRestriction = "12+",
                PosterPath = "/source/matrix-poster.webp",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 10, 5),
                AvailabilityStatus = "ComingSoon"
            },
            new Film
            {
                Id = 16,
                Title = "Початок",
                Genre = "Трилер",
                DurationMinutes = 148,
                Description = "Команда професіоналів занурюється у сни, щоб вкрасти ідеї та посіяти нову думку в підсвідомості.",
                ReleaseYear = 2010,
                AgeRestriction = "12+",
                PosterPath = "/source/images.jpeg",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 10, 18),
                AvailabilityStatus = "ComingSoon"
            },
            new Film
            {
                Id = 17,
                Title = "Кримінальне чтиво",
                Genre = "Кримінал",
                DurationMinutes = 154,
                Description = "Кілька історій із життя злочинного Лос-Анджелеса переплітаються у культовій кримінальній драмі.",
                ReleaseYear = 1994,
                AgeRestriction = "18+",
                PosterPath = "/source/it-poster.jpg",
                TrailerPath = sharedTrailerPath,
                AvailabilityDate = new DateOnly(2026, 11, 1),
                AvailabilityStatus = "ComingSoon"
            }
        ];
    }

    private static IEnumerable<Session> BuildSessionSeed()
    {
        var sessions = new List<Session>
        {
            new() { Id = 1, FilmId = 1, HallId = 1, SessionTime = new DateTime(2026, 7, 3, 9, 30, 0), Price = 180m },
            new() { Id = 2, FilmId = 1, HallId = 1, SessionTime = new DateTime(2026, 7, 3, 12, 45, 0), Price = 180m },
            new() { Id = 3, FilmId = 1, HallId = 2, SessionTime = new DateTime(2026, 7, 3, 17, 30, 0), Price = 220m },
            new() { Id = 4, FilmId = 1, HallId = 2, SessionTime = new DateTime(2026, 7, 3, 19, 45, 0), Price = 220m },
            new() { Id = 5, FilmId = 2, HallId = 1, SessionTime = new DateTime(2026, 7, 4, 9, 30, 0), Price = 180m },
            new() { Id = 6, FilmId = 2, HallId = 1, SessionTime = new DateTime(2026, 7, 4, 12, 45, 0), Price = 180m },
            new() { Id = 7, FilmId = 2, HallId = 2, SessionTime = new DateTime(2026, 7, 4, 15, 55, 0), Price = 220m },
            new() { Id = 8, FilmId = 2, HallId = 2, SessionTime = new DateTime(2026, 7, 4, 19, 45, 0), Price = 220m },
            new() { Id = 9, FilmId = 3, HallId = 1, SessionTime = new DateTime(2026, 7, 5, 9, 30, 0), Price = 160m },
            new() { Id = 10, FilmId = 3, HallId = 1, SessionTime = new DateTime(2026, 7, 5, 12, 45, 0), Price = 160m },
            new() { Id = 11, FilmId = 3, HallId = 2, SessionTime = new DateTime(2026, 7, 5, 15, 55, 0), Price = 190m },
            new() { Id = 12, FilmId = 3, HallId = 2, SessionTime = new DateTime(2026, 7, 5, 17, 30, 0), Price = 190m },
            new() { Id = 13, FilmId = 4, HallId = 1, SessionTime = new DateTime(2026, 7, 6, 9, 30, 0), Price = 160m },
            new() { Id = 14, FilmId = 4, HallId = 2, SessionTime = new DateTime(2026, 7, 6, 15, 55, 0), Price = 190m },
            new() { Id = 15, FilmId = 4, HallId = 2, SessionTime = new DateTime(2026, 7, 6, 17, 30, 0), Price = 190m },
            new() { Id = 16, FilmId = 5, HallId = 1, SessionTime = new DateTime(2026, 7, 7, 9, 30, 0), Price = 170m },
            new() { Id = 17, FilmId = 5, HallId = 1, SessionTime = new DateTime(2026, 7, 7, 12, 45, 0), Price = 170m },
            new() { Id = 18, FilmId = 5, HallId = 2, SessionTime = new DateTime(2026, 7, 7, 15, 55, 0), Price = 200m },
            new() { Id = 19, FilmId = 5, HallId = 2, SessionTime = new DateTime(2026, 7, 7, 17, 30, 0), Price = 200m },
            new() { Id = 20, FilmId = 5, HallId = 3, SessionTime = new DateTime(2026, 7, 7, 19, 45, 0), Price = 260m },
            new() { Id = 21, FilmId = 6, HallId = 1, SessionTime = new DateTime(2026, 7, 8, 12, 45, 0), Price = 180m },
            new() { Id = 22, FilmId = 6, HallId = 2, SessionTime = new DateTime(2026, 7, 8, 15, 55, 0), Price = 220m },
            new() { Id = 23, FilmId = 6, HallId = 3, SessionTime = new DateTime(2026, 7, 8, 19, 45, 0), Price = 280m }
        };

        var nextId = 24;

        nextId = AddRecurringSessions(sessions, nextId, 1, [6, 9, 12, 15, 18, 21, 24], [
            new SessionSlot(1, 10, 0, 180m),
            new SessionSlot(2, 14, 20, 220m),
            new SessionSlot(2, 19, 0, 220m)
        ]);
        nextId = AddRecurringSessions(sessions, nextId, 2, [7, 10, 13, 16, 19, 22, 25], [
            new SessionSlot(1, 10, 15, 180m),
            new SessionSlot(2, 15, 10, 220m),
            new SessionSlot(3, 20, 0, 280m)
        ]);
        nextId = AddRecurringSessions(sessions, nextId, 3, [8, 11, 14, 17, 20, 23, 26], [
            new SessionSlot(1, 11, 0, 160m),
            new SessionSlot(2, 16, 0, 190m),
            new SessionSlot(2, 20, 15, 190m)
        ]);
        nextId = AddRecurringSessions(sessions, nextId, 4, [9, 12, 15, 18, 21, 24, 27], [
            new SessionSlot(1, 10, 40, 160m),
            new SessionSlot(2, 15, 20, 190m),
            new SessionSlot(3, 20, 10, 250m)
        ]);
        nextId = AddRecurringSessions(sessions, nextId, 5, [10, 13, 16, 19, 22, 25, 28], [
            new SessionSlot(1, 11, 20, 170m),
            new SessionSlot(2, 16, 10, 200m),
            new SessionSlot(3, 20, 30, 260m)
        ]);
        nextId = AddRecurringSessions(sessions, nextId, 6, [11, 14, 17, 20, 23, 26, 29], [
            new SessionSlot(1, 10, 30, 180m),
            new SessionSlot(2, 15, 30, 220m),
            new SessionSlot(3, 20, 20, 280m)
        ]);
        nextId = AddRecurringSessions(sessions, nextId, 9, [3, 6, 9, 12, 15, 18, 21, 24], [
            new SessionSlot(1, 10, 10, 170m),
            new SessionSlot(2, 14, 40, 210m),
            new SessionSlot(3, 19, 50, 260m)
        ]);
        nextId = AddRecurringSessions(sessions, nextId, 10, [4, 7, 10, 13, 16, 19, 22, 25], [
            new SessionSlot(1, 9, 50, 190m),
            new SessionSlot(2, 14, 30, 230m),
            new SessionSlot(3, 19, 40, 270m)
        ]);
        nextId = AddRecurringSessions(sessions, nextId, 11, [5, 8, 11, 14, 17, 20, 23, 26], [
            new SessionSlot(1, 10, 20, 160m),
            new SessionSlot(2, 15, 0, 200m),
            new SessionSlot(2, 18, 50, 200m)
        ]);
        nextId = AddRecurringSessions(sessions, nextId, 12, [6, 9, 12, 15, 18, 21, 24, 27], [
            new SessionSlot(1, 9, 40, 150m),
            new SessionSlot(2, 13, 50, 180m),
            new SessionSlot(3, 18, 30, 220m)
        ]);
        AddRecurringSessions(sessions, nextId, 13, [7, 10, 13, 16, 19, 22, 25, 28], [
            new SessionSlot(1, 10, 50, 210m),
            new SessionSlot(2, 15, 40, 250m),
            new SessionSlot(3, 20, 25, 300m)
        ]);

        return sessions;
    }

    private static int AddRecurringSessions(
        ICollection<Session> sessions,
        int nextId,
        int filmId,
        IEnumerable<int> julyDays,
        IEnumerable<SessionSlot> slots)
    {
        foreach (var day in julyDays)
        {
            foreach (var slot in slots)
            {
                sessions.Add(new Session
                {
                    Id = nextId++,
                    FilmId = filmId,
                    HallId = slot.HallId,
                    SessionTime = new DateTime(2026, 7, day, slot.Hour, slot.Minute, 0),
                    Price = slot.Price
                });
            }
        }

        return nextId;
    }

    private sealed record SessionSlot(int HallId, int Hour, int Minute, decimal Price);
}
