using QuestPDF.Fluent;
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
            page.Margin(40);
            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer()
                .AlignCenter()
                .Text("Thank you for booking with us – Travel Web App");
        });
    }

    void ComposeHeader(IContainer container)
    {
        var bookingDateText = _booking.BookingDate != default
            ? _booking.BookingDate.ToString("dd/MM/yyyy HH:mm")
            : "N/A";

        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("Booking Confirmation").FontSize(20).Bold();
                col.Item().Text($"Booking ID: {_booking.Id}");
                col.Item().Text($"Date: {bookingDateText}");
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        var customer = _booking.Customer;
        var trip = _booking.Trip;

        container.PaddingVertical(20).Column(col =>
        {
            // ---------------- CUSTOMER ----------------
            col.Item().Text("Customer Information").Bold().FontSize(16);
            col.Item().Text($"Name: {customer?.FullName ?? "N/A"}");
            col.Item().Text($"Email: {customer?.Email ?? "N/A"}");
            col.Item().Text($"Phone: {customer?.Phone ?? "N/A"}");
            col.Item().PaddingTop(15);

            // ---------------- TRIP ----------------
            col.Item().Text("Trip Information").Bold().FontSize(16);
            col.Item().Text($"Trip: {trip?.Title ?? "N/A"}");
            col.Item().Text($"Destination: {trip?.Destination?.Name ?? "N/A"}");
            col.Item().Text($"Transport: {trip?.TransportType.ToString() ?? "N/A"}");

            string dateRange = "N/A";
            if (trip != null)
            {
                var from = trip.DepartureDate.ToString("dd/MM/yyyy");
                var to = trip.ReturnDate?.ToString("dd/MM/yyyy") ?? "-";
                dateRange = $"{from} - {to}";
            }
            col.Item().Text($"Dates: {dateRange}");
            col.Item().PaddingTop(15);

            // ---------------- BOOKING DETAILS ----------------
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

                var pricePerPersonText = trip != null
                    ? $"{trip.PricePerPerson:0.00} €"
                    : "N/A";

                var totalPriceText = _booking.TotalPrice > 0
                    ? $"{_booking.TotalPrice:0.00} €"
                    : "N/A";

                table.Cell().Text(pricePerPersonText);
                table.Cell().Text(totalPriceText);
            });
        });
    }
}
