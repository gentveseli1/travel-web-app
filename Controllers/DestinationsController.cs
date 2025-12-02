using Microsoft.AspNetCore.Mvc;
using TravelWebApp.Models;
using TravelWebApp.Repositories;

namespace TravelWebApp.Controllers
{
    public class DestinationsController : Controller
    {
        private readonly IDestinationRepository _repo;

        public DestinationsController(IDestinationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Destination destination)
        {
            if (!ModelState.IsValid)
                return View(destination);

            await _repo.AddAsync(destination);
            await _repo.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var destination = await _repo.GetByIdAsync(id);
            if (destination == null)
                return NotFound();

            return View(destination);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Destination destination)
        {
            if (id != destination.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(destination);

            await _repo.UpdateAsync(destination);
            await _repo.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var destination = await _repo.GetByIdAsync(id);
            if (destination == null)
                return NotFound();

            return View(destination);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
