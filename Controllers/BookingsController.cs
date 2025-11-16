using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelWebApp.Data;
using TravelWebApp.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace TravelWebApp.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Trip)
                .ToListAsync();

            return View(bookings);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var trip = await _context.Trips.FindAsync(booking.TripId);

                if (trip == null)
                {
                    ModelState.AddModelError("TripId", "Selected trip does not exist.");
                    LoadDropdowns();
                    return View(booking);
                }

                booking.TotalPrice = booking.NumberOfPassengers * trip.PricePerPerson;
                booking.BookingDate = DateTime.UtcNow;

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(booking);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            LoadDropdowns(booking.CustomerId, booking.TripId, booking.Status);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var trip = await _context.Trips.FindAsync(booking.TripId);
                if (trip == null)
                {
                    ModelState.AddModelError("TripId", "Selected trip does not exist.");
                    LoadDropdowns(booking.CustomerId, booking.TripId, booking.Status);
                    return View(booking);
                }

                booking.TotalPrice = booking.NumberOfPassengers * trip.PricePerPerson;

                _context.Update(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(booking.CustomerId, booking.TripId, booking.Status);
            return View(booking);
        }

        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Trip).ThenInclude(t => t.Destination)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Trip)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DownloadPdf(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Trip).ThenInclude(t => t.Destination)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            var document = new BookingPdfDocument(booking);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Booking_{booking.Id}.pdf");
        }

        private void LoadDropdowns(int? customerId = null, int? tripId = null, BookingStatus? status = null)
        {
            ViewBag.Trips = new SelectList(_context.Trips.OrderBy(t => t.Title), "Id", "Title", tripId);
            ViewBag.Customers = new SelectList(_context.Customers.OrderBy(c => c.FullName), "Id", "FullName", customerId);

            ViewBag.Statuses = Enum.GetValues(typeof(BookingStatus))
                .Cast<BookingStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString(),
                    Selected = (status != null && status == s)
                })
                .ToList();
        }
    }
}