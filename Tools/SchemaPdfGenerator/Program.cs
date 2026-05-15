using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;

const string outputPath = "/Users/anastasiiakrasheninnikova/Documents/NURE/diploma/CinemaPlus_CinemaWebApp/database-structure.pdf";

GlobalFontSettings.FontResolver = new SchemaPdfFontResolver();

var tables = new List<TableSchema>
{
    new(
        "Users",
        "Stores registered system users, including admins and clients.",
        [
            new("Id", "int", "PK, identity, required"),
            new("FullName", "varchar(150)", "required"),
            new("Email", "varchar(180)", "required, unique"),
            new("Phone", "varchar(30)", "nullable"),
            new("ProfilePhotoPath", "varchar(300)", "nullable"),
            new("PasswordHash", "varchar(255)", "required"),
            new("Role", "varchar(30)", "required"),
            new("CreatedAt", "datetime", "required")
        ],
        [
            "Unique index: Email",
            "Referenced by Bookings.UserId"
        ]),
    new(
        "Films",
        "Movie catalogue entries available now or coming soon.",
        [
            new("Id", "int", "PK, identity, required"),
            new("Title", "varchar(200)", "required, unique"),
            new("Genre", "varchar(100)", "required"),
            new("DurationMinutes", "int", "required"),
            new("Description", "text", "required"),
            new("ReleaseYear", "int", "required"),
            new("AgeRestriction", "varchar(20)", "required"),
            new("PosterPath", "varchar(300)", "required"),
            new("TrailerPath", "varchar(300)", "nullable"),
            new("AvailabilityDate", "date", "required"),
            new("AvailabilityStatus", "varchar(40)", "required")
        ],
        [
            "Unique index: Title",
            "Composite index: AvailabilityStatus + AvailabilityDate",
            "Referenced by Sessions.FilmId"
        ]),
    new(
        "Halls",
        "Cinema halls with technology and seat-layout parameters.",
        [
            new("Id", "int", "PK, identity, required"),
            new("Name", "varchar(80)", "required, unique"),
            new("Technology", "varchar(80)", "required"),
            new("RowsCount", "int", "required"),
            new("SeatsPerRow", "int", "required")
        ],
        [
            "Unique index: Name",
            "Referenced by Seats.HallId and Sessions.HallId"
        ]),
    new(
        "Seats",
        "Physical seat map for every hall.",
        [
            new("Id", "int", "PK, identity, required"),
            new("HallId", "int", "FK -> Halls.Id, required"),
            new("RowNumber", "int", "required"),
            new("SeatNumber", "int", "required")
        ],
        [
            "Unique index: HallId + RowNumber + SeatNumber",
            "Cascade delete from Halls",
            "Referenced by BookedSeats.SeatId"
        ]),
    new(
        "Sessions",
        "Concrete showtimes for films in halls.",
        [
            new("Id", "int", "PK, identity, required"),
            new("FilmId", "int", "FK -> Films.Id, required"),
            new("HallId", "int", "FK -> Halls.Id, required"),
            new("SessionTime", "datetime", "required"),
            new("Price", "decimal(10,2)", "required")
        ],
        [
            "Unique index: FilmId + HallId + SessionTime",
            "Index: SessionTime",
            "Delete restricted from Films and Halls",
            "Referenced by Bookings.SessionId and BookedSeats.SessionId"
        ]),
    new(
        "Bookings",
        "Customer orders for one session, optionally linked to a user account.",
        [
            new("Id", "int", "PK, identity, required"),
            new("UserId", "int", "FK -> Users.Id, nullable"),
            new("SessionId", "int", "FK -> Sessions.Id, required"),
            new("BookingDate", "datetime", "required"),
            new("Status", "varchar(40)", "required"),
            new("TotalPrice", "decimal(10,2)", "required"),
            new("TicketCode", "varchar(80)", "required, unique"),
            new("CustomerName", "varchar(150)", "required"),
            new("CustomerEmail", "varchar(180)", "required"),
            new("CustomerPhone", "varchar(30)", "nullable")
        ],
        [
            "Unique index: TicketCode",
            "Index: BookingDate",
            "User delete behavior: SET NULL",
            "Session delete behavior: RESTRICT",
            "Referenced by BookedSeats.BookingId and NotificationLogs.BookingId"
        ]),
    new(
        "BookedSeats",
        "Bridge table that reserves specific seats for a booking within a session.",
        [
            new("Id", "int", "PK, identity, required"),
            new("BookingId", "int", "FK -> Bookings.Id, required"),
            new("SessionId", "int", "FK -> Sessions.Id, required"),
            new("SeatId", "int", "FK -> Seats.Id, required")
        ],
        [
            "Unique index: SessionId + SeatId",
            "Unique index: BookingId + SeatId",
            "Booking delete behavior: CASCADE",
            "Session and Seat delete behavior: RESTRICT"
        ]),
    new(
        "NotificationLogs",
        "Audit trail of booking-related notifications and emails.",
        [
            new("Id", "int", "PK, identity, required"),
            new("BookingId", "int", "FK -> Bookings.Id, required"),
            new("Email", "varchar(180)", "required"),
            new("Message", "text", "required"),
            new("CreatedDate", "datetime", "required"),
            new("Status", "varchar(40)", "required")
        ],
        [
            "Booking delete behavior: CASCADE"
        ])
};

var relations = new List<RelationSchema>
{
    new("Users", "Bookings", "1 -> many", "Bookings.UserId is nullable for guest orders; on user delete value becomes NULL."),
    new("Films", "Sessions", "1 -> many", "Deleting a film is restricted while sessions exist."),
    new("Halls", "Seats", "1 -> many", "Deleting a hall cascades to its seat map."),
    new("Halls", "Sessions", "1 -> many", "Deleting a hall is restricted while sessions exist."),
    new("Sessions", "Bookings", "1 -> many", "Each booking belongs to exactly one session."),
    new("Bookings", "BookedSeats", "1 -> many", "A booking may contain multiple seats."),
    new("Sessions", "BookedSeats", "1 -> many", "Used to enforce unique sold seat per session."),
    new("Seats", "BookedSeats", "1 -> many", "Physical seats can appear in many historical bookings across sessions."),
    new("Bookings", "NotificationLogs", "1 -> many", "Notification history is removed when a booking is deleted.")
};

using var document = new PdfDocument();
document.Info.Title = "CinemaPlus Database Structure";
document.Info.Author = "OpenAI Codex";

var page = document.AddPage();
page.Size = PdfSharpCore.PageSize.A4;
var graphics = XGraphics.FromPdfPage(page);

var titleFont = new XFont("SchemaSans", 20, XFontStyle.Bold);
var headingFont = new XFont("SchemaSans", 12, XFontStyle.Bold);
var bodyFont = new XFont("SchemaSans", 9, XFontStyle.Regular);
var smallBoldFont = new XFont("SchemaSans", 8, XFontStyle.Bold);
var smallFont = new XFont("SchemaSans", 8, XFontStyle.Regular);

var ink = XBrushes.Black;
var accentBrush = new XSolidBrush(XColor.FromArgb(24, 61, 115));
var subtleBrush = new XSolidBrush(XColor.FromArgb(77, 88, 110));
var headerFill = new XSolidBrush(XColor.FromArgb(225, 235, 247));
var borderPen = new XPen(XColor.FromArgb(180, 190, 210), 0.8);
var lightPen = new XPen(XColor.FromArgb(220, 226, 236), 0.5);

double margin = 36;
double y = 34;

graphics.DrawString("CinemaPlus Database Structure", titleFont, accentBrush, new XRect(margin, y, page.Width - margin * 2, 24), XStringFormats.TopLeft);
y += 26;
graphics.DrawString("Tables, key columns, constraints, and current relationships.", bodyFont, subtleBrush, new XRect(margin, y, page.Width - margin * 2, 16), XStringFormats.TopLeft);
y += 22;

foreach (var table in tables)
{
    var estimatedHeight = EstimateTableHeight(table);
    if (y + estimatedHeight > page.Height - margin)
    {
        graphics.Dispose();
        page = document.AddPage();
        page.Size = PdfSharpCore.PageSize.A4;
        graphics = XGraphics.FromPdfPage(page);
        y = margin;
    }

    DrawTableSchema(graphics, table, margin, y, page.Width - margin * 2, headingFont, bodyFont, smallBoldFont, smallFont, ink, subtleBrush, accentBrush, headerFill, borderPen, lightPen);
    y += estimatedHeight + 12;
}

graphics.Dispose();

var relationPage = document.AddPage();
relationPage.Size = PdfSharpCore.PageSize.A4;
using var relationGraphics = XGraphics.FromPdfPage(relationPage);
double relationY = margin;

relationGraphics.DrawString("Table Relations", titleFont, accentBrush, new XRect(margin, relationY, relationPage.Width - margin * 2, 24), XStringFormats.TopLeft);
relationY += 28;
relationGraphics.DrawString("Logical relations currently implemented in the EF Core model.", bodyFont, subtleBrush, new XRect(margin, relationY, relationPage.Width - margin * 2, 16), XStringFormats.TopLeft);
relationY += 20;

DrawRelationsTable(relationGraphics, relations, margin, relationY, relationPage.Width - margin * 2, headingFont, bodyFont, smallBoldFont, smallFont, ink, subtleBrush, headerFill, borderPen, lightPen);

Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
using var output = File.Create(outputPath);
document.Save(output, false);

Console.WriteLine($"Created {outputPath}");

static double EstimateTableHeight(TableSchema table)
{
    return 52 + table.Columns.Count * 18 + table.Notes.Count * 12 + 18;
}

static void DrawTableSchema(
    XGraphics g,
    TableSchema table,
    double x,
    double y,
    double width,
    XFont headingFont,
    XFont bodyFont,
    XFont smallBoldFont,
    XFont smallFont,
    XBrush ink,
    XBrush subtleBrush,
    XBrush accentBrush,
    XBrush headerFill,
    XPen borderPen,
    XPen lightPen)
{
    var height = EstimateTableHeight(table);
    g.DrawRectangle(borderPen, x, y, width, height);
    g.DrawRectangle(headerFill, x, y, width, 24);
    g.DrawString(table.Name, headingFont, ink, new XRect(x + 8, y + 4, width - 16, 16), XStringFormats.TopLeft);
    g.DrawString(table.Description, smallFont, subtleBrush, new XRect(x + 8, y + 28, width - 16, 20), XStringFormats.TopLeft);

    var headerY = y + 48;
    g.DrawLine(borderPen, x, headerY, x + width, headerY);
    g.DrawString("Column", smallBoldFont, ink, new XRect(x + 8, headerY + 3, 120, 12), XStringFormats.TopLeft);
    g.DrawString("Type", smallBoldFont, ink, new XRect(x + 150, headerY + 3, 120, 12), XStringFormats.TopLeft);
    g.DrawString("Rules", smallBoldFont, ink, new XRect(x + 275, headerY + 3, width - 283, 12), XStringFormats.TopLeft);

    var rowY = headerY + 18;
    foreach (var column in table.Columns)
    {
        g.DrawLine(lightPen, x, rowY, x + width, rowY);
        g.DrawString(column.Name, smallFont, ink, new XRect(x + 8, rowY + 3, 130, 12), XStringFormats.TopLeft);
        g.DrawString(column.Type, smallFont, ink, new XRect(x + 150, rowY + 3, 115, 12), XStringFormats.TopLeft);
        g.DrawString(column.Rules, smallFont, ink, new XRect(x + 275, rowY + 3, width - 283, 12), XStringFormats.TopLeft);
        rowY += 18;
    }

    g.DrawString("Constraints / notes", smallBoldFont, accentBrush, new XRect(x + 8, rowY + 6, 160, 12), XStringFormats.TopLeft);
    rowY += 20;

    foreach (var note in table.Notes)
    {
        g.DrawString($"- {note}", smallFont, subtleBrush, new XRect(x + 10, rowY, width - 20, 12), XStringFormats.TopLeft);
        rowY += 12;
    }
}

static void DrawRelationsTable(
    XGraphics g,
    IReadOnlyList<RelationSchema> relations,
    double x,
    double y,
    double width,
    XFont headingFont,
    XFont bodyFont,
    XFont smallBoldFont,
    XFont smallFont,
    XBrush ink,
    XBrush subtleBrush,
    XBrush headerFill,
    XPen borderPen,
    XPen lightPen)
{
    var rowHeight = 28d;
    var totalHeight = 24 + rowHeight * (relations.Count + 1);
    g.DrawRectangle(borderPen, x, y, width, totalHeight);
    g.DrawRectangle(headerFill, x, y, width, 24);
    g.DrawString("Current Entity Relationships", headingFont, ink, new XRect(x + 8, y + 4, width - 16, 16), XStringFormats.TopLeft);

    var headerY = y + 24;
    g.DrawString("Parent", smallBoldFont, ink, new XRect(x + 8, headerY + 7, 90, 12), XStringFormats.TopLeft);
    g.DrawString("Child", smallBoldFont, ink, new XRect(x + 108, headerY + 7, 90, 12), XStringFormats.TopLeft);
    g.DrawString("Cardinality", smallBoldFont, ink, new XRect(x + 208, headerY + 7, 80, 12), XStringFormats.TopLeft);
    g.DrawString("Meaning", smallBoldFont, ink, new XRect(x + 300, headerY + 7, width - 308, 12), XStringFormats.TopLeft);

    var rowY = headerY + rowHeight;
    foreach (var relation in relations)
    {
        g.DrawLine(lightPen, x, rowY, x + width, rowY);
        g.DrawString(relation.Parent, smallFont, ink, new XRect(x + 8, rowY + 7, 90, 12), XStringFormats.TopLeft);
        g.DrawString(relation.Child, smallFont, ink, new XRect(x + 108, rowY + 7, 90, 12), XStringFormats.TopLeft);
        g.DrawString(relation.Cardinality, smallFont, ink, new XRect(x + 208, rowY + 7, 82, 12), XStringFormats.TopLeft);
        g.DrawString(relation.Meaning, smallFont, subtleBrush, new XRect(x + 300, rowY + 5, width - 308, 18), XStringFormats.TopLeft);
        rowY += rowHeight;
    }
}

internal sealed class SchemaPdfFontResolver : IFontResolver
{
    private const string Family = "SchemaSans";
    private const string Regular = "SchemaSans-Regular";
    private const string Bold = "SchemaSans-Bold";

    private static readonly Lazy<byte[]> RegularFont = new(() =>
        File.ReadAllBytes("/System/Library/Fonts/Supplemental/Arial.ttf"));

    private static readonly Lazy<byte[]> BoldFont = new(() =>
        File.ReadAllBytes("/System/Library/Fonts/Supplemental/Arial Bold.ttf"));

    public string DefaultFontName => Family;

    public byte[]? GetFont(string faceName) => faceName switch
    {
        Regular => RegularFont.Value,
        Bold => BoldFont.Value,
        _ => null
    };

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        => new(isBold ? Bold : Regular);
}

internal sealed record ColumnSchema(string Name, string Type, string Rules);
internal sealed record TableSchema(string Name, string Description, IReadOnlyList<ColumnSchema> Columns, IReadOnlyList<string> Notes);
internal sealed record RelationSchema(string Parent, string Child, string Cardinality, string Meaning);
