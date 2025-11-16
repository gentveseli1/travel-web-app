using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TravelWebApp.Models;

public class BookingPdfDocument : IDocument
{
    private readonly Booking _booking;

    public BookingPdfDocument(Booking booking)
    {
        _booking = booking;
    }

    public DocumentMetadata GetMetadata() => new DocumentMetadata
    {
        Title = $"Booking {_booking.Id}"
    };

    public DocumentSettings GetSettings() => new DocumentSettings
    {
        PdfA = false
    };

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);   // ✔ margjinat vendosen ketu
            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().AlignCenter().Text("Thank you for booking with us – Travel Web App");
        });
    }

    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("Booking Confirmation").FontSize(20).Bold();
                col.Item().Text($"Booking ID: {_booking.Id}");
                col.Item().Text($"Date: {_booking.BookingDate:dd/MM/yyyy HH:mm}");
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(20).Column(col =>
        {
            col.Item().Text("Customer Information").Bold().FontSize(16);
            col.Item().Text($"Name: {_booking.Customer.FullName}");
            col.Item().Text($"Email: {_booking.Customer.Email}");
            col.Item().Text($"Phone: {_booking.Customer.Phone}");
            col.Item().PaddingTop(15);

            col.Item().Text("Trip Information").Bold().FontSize(16);
            col.Item().Text($"Trip: {_booking.Trip.Title}");
            col.Item().Text($"Destination: {_booking.Trip.Destination?.Name}");
            col.Item().Text($"Transport: {_booking.Trip.TransportType}");
            col.Item().Text($"Dates: {_booking.Trip.DepartureDate:dd/MM/yyyy} - {_booking.Trip.ReturnDate?.ToString("dd/MM/yyyy")}");
            col.Item().PaddingTop(15);

            col.Item().Text("Booking Details").Bold().FontSize(16);

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Text("Passengers").Bold();
                    header.Cell().Text("Price/Person").Bold();
                    header.Cell().Text("Total").Bold();
                });

                table.Cell().Text(_booking.NumberOfPassengers.ToString());
                table.Cell().Text($"{_booking.Trip.PricePerPerson} €");
                table.Cell().Text($"{_booking.TotalPrice} €");
            });
        });
    }
}
