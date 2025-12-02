using TravelWebApp.Models;

namespace TravelWebApp.Repositories
{
public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllWithIncludesAsync();
        Task<Booking?> GetByIdWithIncludesAsync(int id);
        Task<Booking?> GetByIdAsync(int id);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
