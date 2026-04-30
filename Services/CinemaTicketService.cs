using System.Globalization;
using CinemaPlus.CinemaWebApp.ViewModels;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using QRCoder;

namespace CinemaPlus.CinemaWebApp.Services;

public class CinemaTicketService : ICinemaTicketService
{
    private const string PdfFontFamily = "CinemaPlusSans";
    private static readonly object FontResolverLock = new();
    private static bool fontResolverConfigured;

    public string GenerateTicketCode()
    {
        return $"CP-{Guid.NewGuid().ToString("N")[..10].ToUpperInvariant()}";
    }

    public string FormatOrderNumber(int bookingId, DateTime bookingDate)
    {
        return $"{bookingDate:yyMMdd}{bookingId:D4}";
    }

    public string BuildQrPayload(TicketConfirmationViewModel ticket)
    {
        return string.Join(
            Environment.NewLine,
            [
                "CinemaPlus",
                $"Код квитка: {ticket.TicketCode}",
                $"Номер замовлення: {ticket.OrderNumber}",
                $"Фільм: {ticket.MovieTitle}",
                $"Дата: {ticket.DateText}",
                $"Сеанс: {ticket.TimeText}",
                $"Зал: {ticket.HallName} ({ticket.Technology})",
                $"Місця: {ticket.SeatSummaryText}",
                $"Email: {ticket.CustomerEmail}"
            ]);
    }

    public string GenerateQrCodeDataUri(string payload)
    {
        var pngBytes = GenerateQrCodePng(payload);
        return $"data:image/png;base64,{Convert.ToBase64String(pngBytes)}";
    }

    public byte[] GenerateTicketPdf(TicketConfirmationViewModel ticket)
    {
        EnsureFontResolver();

        using var document = new PdfDocument();
        document.Info.Title = $"Квиток CinemaPlus {ticket.OrderNumber}";
        document.Info.Author = "CinemaPlus";

        var page = document.AddPage();
        page.Size = PdfSharpCore.PageSize.A4;

        using var graphics = XGraphics.FromPdfPage(page);

        var titleFont = new XFont(PdfFontFamily, 20, XFontStyle.Bold);
        var sectionFont = new XFont(PdfFontFamily, 12, XFontStyle.Bold);
        var bodyFont = new XFont(PdfFontFamily, 11, XFontStyle.Regular);
        var smallFont = new XFont(PdfFontFamily, 10, XFontStyle.Regular);
        var accentBrush = new XSolidBrush(XColor.FromArgb(9, 56, 106));
        var mutedBrush = new XSolidBrush(XColor.FromArgb(84, 98, 120));
        var borderPen = new XPen(XColor.FromArgb(208, 215, 227), 1);
        var panelBrush = new XSolidBrush(XColor.FromArgb(246, 248, 252));

        graphics.DrawRectangle(panelBrush, 32, 32, page.Width - 64, page.Height - 64);
        graphics.DrawRectangle(borderPen, 32, 32, page.Width - 64, page.Height - 64);

        graphics.DrawString("КВИТОК CINEMAPLUS", titleFont, accentBrush, new XRect(52, 48, page.Width - 104, 28), XStringFormats.TopLeft);
        graphics.DrawString("Покажіть QR-код на вході до кінозалу", smallFont, mutedBrush, new XRect(52, 80, page.Width - 104, 20), XStringFormats.TopLeft);

        graphics.DrawString(ticket.MovieTitle, titleFont, XBrushes.Black, new XRect(52, 114, 320, 42), XStringFormats.TopLeft);
        graphics.DrawString($"{ticket.DurationMinutes} хв • {ticket.Genre}", bodyFont, mutedBrush, new XRect(52, 156, 320, 20), XStringFormats.TopLeft);

        DrawLabelValue(graphics, sectionFont, bodyFont, "Номер замовлення", ticket.OrderNumber, 52, 202);
        DrawLabelValue(graphics, sectionFont, bodyFont, "Код квитка", ticket.TicketCode, 52, 242);
        DrawLabelValue(graphics, sectionFont, bodyFont, "Дата", ticket.DateText, 52, 282);
        DrawLabelValue(graphics, sectionFont, bodyFont, "Сеанс", ticket.TimeText, 52, 322);
        DrawLabelValue(graphics, sectionFont, bodyFont, "Зал", $"{ticket.HallName} ({ticket.Technology})", 52, 362);
        DrawLabelValue(graphics, sectionFont, bodyFont, "Email", ticket.CustomerEmail, 52, 402);
        DrawLabelValue(
            graphics,
            sectionFont,
            bodyFont,
            "Вартість",
            $"{ticket.TotalPrice.ToString("0.00", CultureInfo.InvariantCulture)} грн",
            52,
            442);

        graphics.DrawString("Місця", sectionFont, accentBrush, new XRect(52, 484, 150, 18), XStringFormats.TopLeft);
        graphics.DrawString(ticket.SeatSummaryText, bodyFont, XBrushes.Black, new XRect(52, 506, 300, 78), XStringFormats.TopLeft);

        graphics.DrawRectangle(borderPen, panelBrush, 382, 114, 160, 160);

        var qrBytes = GenerateQrCodePng(ticket.QrPayload);
        using var qrImage = XImage.FromStream(() => new MemoryStream(qrBytes, writable: false));
        graphics.DrawImage(qrImage, 397, 129, 130, 130);

        graphics.DrawString("QR-КОД", sectionFont, accentBrush, new XRect(382, 286, 160, 18), XStringFormats.TopCenter);
        graphics.DrawString(
            "Скануйте код або покажіть PDF на вході до залу.",
            smallFont,
            mutedBrush,
            new XRect(382, 312, 160, 46),
            XStringFormats.TopCenter);

        graphics.DrawLine(borderPen, 52, 604, page.Width - 52, 604);
        graphics.DrawString(
            "CinemaPlus • вул. Кінематографічна, 5, м. Харків",
            smallFont,
            mutedBrush,
            new XRect(52, 616, page.Width - 104, 20),
            XStringFormats.TopLeft);

        using var stream = new MemoryStream();
        document.Save(stream, false);
        return stream.ToArray();
    }

    private static void EnsureFontResolver()
    {
        if (fontResolverConfigured)
        {
            return;
        }

        lock (FontResolverLock)
        {
            if (fontResolverConfigured)
            {
                return;
            }

            GlobalFontSettings.FontResolver = new CinemaPlusFontResolver();
            fontResolverConfigured = true;
        }
    }

    private static byte[] GenerateQrCodePng(string payload)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrData);
        return qrCode.GetGraphic(8, drawQuietZones: true);
    }

    private static void DrawLabelValue(
        XGraphics graphics,
        XFont labelFont,
        XFont valueFont,
        string label,
        string value,
        double x,
        double y)
    {
        graphics.DrawString(label.ToUpper(CultureInfo.InvariantCulture), labelFont, XBrushes.Black, new XRect(x, y, 180, 18), XStringFormats.TopLeft);
        graphics.DrawString(value, valueFont, XBrushes.Black, new XRect(x, y + 20, 280, 18), XStringFormats.TopLeft);
    }
}
