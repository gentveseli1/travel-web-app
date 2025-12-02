using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TravelWebApp.Models;
using TravelWebApp.Repositories;

namespace TravelWebApp.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly ITripRepository _tripRepo;

        public BookingsController(
            IBookingRepository bookingRepo,
            ICustomerRepository customerRepo,
            ITripRepository tripRepo)
        {
            _bookingRepo = bookingRepo;
            _customerRepo = customerRepo;
            _tripRepo = tripRepo;
        }

        // GET: /Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingRepo.GetAllWithIncludesAsync();
            return View(bookings);
        }

        // GET: /Bookings/Create
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        // POST: /Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return View(booking);
            }

            var trip = await _tripRepo.GetByIdAsync(booking.TripId);
            if (trip == null)
            {
                ModelState.AddModelError("TripId", "Trip not found.");
                await LoadDropdownsAsync();
                return View(booking);
            }

            booking.TotalPrice = booking.NumberOfPassengers * trip.PricePerPerson;
            booking.BookingDate = DateTime.UtcNow;

            await _bookingRepo.AddAsync(booking);
            await _bookingRepo.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Bookings/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _bookingRepo.GetByIdAsync(id);
            if (booking == null)
                return NotFound();

            await LoadDropdownsAsync(booking.CustomerId, booking.TripId, booking.Status);
            return View(booking);
        }

        // POST: /Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return View(booking);
            }

            var trip = await _tripRepo.GetByIdAsync(booking.TripId);
            if (trip == null)
            {
                ModelState.AddModelError("TripId", "Trip not found.");
                await LoadDropdownsAsync();
                return View(booking);
            }

            booking.TotalPrice = booking.NumberOfPassengers * trip.PricePerPerson;

            await _bookingRepo.UpdateAsync(booking);
            await _bookingRepo.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Bookings/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingRepo.GetByIdWithIncludesAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // GET: /Bookings/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _bookingRepo.GetByIdWithIncludesAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: /Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookingRepo.DeleteAsync(id);
            await _bookingRepo.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdownsAsync(
            int? customerId = null,
            int? tripId = null,
            BookingStatus? status = null)
        {
            var trips = await _tripRepo.GetAllAsync();
            var customers = await _customerRepo.GetAllAsync();

            ViewBag.Trips = new SelectList(trips, "Id", "Title", tripId);
            ViewBag.Customers = new SelectList(customers, "Id", "FullName", customerId);

            ViewBag.Statuses = Enum.GetValues(typeof(BookingStatus))
                .Cast<BookingStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString(),
                    Selected = (status == s)
                })
                .ToList();
        }
    }
}
