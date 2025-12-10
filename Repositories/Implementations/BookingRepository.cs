using Microsoft.EntityFrameworkCore;
using TravelWebApp.Data;
using TravelWebApp.Models;

namespace TravelWebApp.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllWithIncludesAsync()
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.Destination)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.Destination)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.Destination)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Bookings.FindAsync(id);
            if (entity != null)
                _context.Bookings.Remove(entity);
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
