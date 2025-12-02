using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TravelWebApp.Models;
using TravelWebApp.Repositories;

namespace TravelWebApp.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripRepository _tripRepo;
        private readonly IDestinationRepository _destinationRepo;

        public TripsController(ITripRepository tripRepo, IDestinationRepository destinationRepo)
        {
            _tripRepo = tripRepo;
            _destinationRepo = destinationRepo;
        }

        public async Task<IActionResult> Index()
        {
            var trips = await _tripRepo.GetAllAsync();
            return View(trips);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trip trip)
        {
            if (trip.AvailableSeats == 0)
                trip.AvailableSeats = trip.TotalSeats;

            if (trip.AvailableSeats > trip.TotalSeats)
            {
                ModelState.AddModelError(nameof(trip.AvailableSeats),
                    "Available seats cannot be greater than total seats.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return View(trip);
            }

            await _tripRepo.AddAsync(trip);
            await _tripRepo.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var trip = await _tripRepo.GetByIdAsync(id);
            if (trip == null)
                return NotFound();

            await LoadDropdownsAsync();
            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trip trip)
        {
            if (id != trip.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return View(trip);
            }

            await _tripRepo.UpdateAsync(trip);
            await _tripRepo.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var trip = await _tripRepo.GetByIdAsync(id);
            if (trip == null)
                return NotFound();

            return View(trip);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var trip = await _tripRepo.GetByIdAsync(id);
            if (trip == null)
                return NotFound();

            return View(trip);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tripRepo.DeleteAsync(id);
            await _tripRepo.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdownsAsync()
        {
            var destinations = await _destinationRepo.GetAllAsync();

            ViewBag.Destinations = new SelectList(destinations, "Id", "Name");

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
