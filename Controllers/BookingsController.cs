using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelWebApp.Data;
using TravelWebApp.Models;

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

            LoadDropdowns();
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
                if (trip != null)
                {
                    booking.TotalPrice = booking.NumberOfPassengers * trip.PricePerPerson;
                }

                _context.Update(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(booking);
        }

        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Trip)
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

        private void LoadDropdowns(int? customerId = null, int? tripId = null, BookingStatus? status = null)
        {
            ViewBag.Trips = new SelectList(_context.Trips, "Id", "Title", tripId);
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "FullName", customerId);

            ViewBag.Statuses =
                Enum.GetValues(typeof(BookingStatus))
                .Cast<BookingStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString(),
                    Selected = status.HasValue && status.Value == s
                })
                .ToList();
        }
    }
}
