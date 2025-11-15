using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelWebApp.Data;
using TravelWebApp.Models;

namespace TravelWebApp.Controllers
{
    public class TripsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TripsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var trips = await _context.Trips
                .Include(t => t.Destination)
                .ToListAsync();

            return View(trips);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trip trip)
        {
            if (trip.AvailableSeats == 0)
            {
                trip.AvailableSeats = trip.TotalSeats;
            }

            if (trip.AvailableSeats > trip.TotalSeats)
            {
                ModelState.AddModelError(nameof(trip.AvailableSeats),
                    "Available seats cannot be greater than total seats.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(trip);
            }

            _context.Add(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
                return NotFound();

            LoadDropdowns();
            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trip trip)
        {
            if (id != trip.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(trip);
        }

        public async Task<IActionResult> Details(int id)
        {
            var trip = await _context.Trips
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null)
                return NotFound();

            return View(trip);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var trip = await _context.Trips
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null)
                return NotFound();

            return View(trip);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns()
        {
            ViewBag.Destinations = new SelectList(_context.Destinations, "Id", "Name");

            ViewBag.TransportTypes = Enum.GetValues(typeof(TransportType))
                .Cast<TransportType>()
                .Select(x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = x.ToString()
                })
                .ToList();
        }

    }
}
