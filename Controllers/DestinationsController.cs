using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWebApp.Data;
using TravelWebApp.Models;

namespace TravelWebApp.Controllers
{
    public class DestinationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DestinationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.Destinations.ToListAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Destination destination)
        {
            if (!ModelState.IsValid)
                return View(destination);

            _context.Add(destination);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();
            return View(destination);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Destination destination)
        {
            if (id != destination.Id) return NotFound();
            if (!ModelState.IsValid) return View(destination);

            _context.Update(destination);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            return View(destination);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
