using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWebApp.Data;
using TravelWebApp.Models;

namespace TravelWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalTrips = await _context.Trips.CountAsync();
            var totalCustomers = await _context.Customers.CountAsync();
            var totalBookings = await _context.Bookings.CountAsync();
            var activeBookings = await _context.Bookings
                .CountAsync(b => b.Status == BookingStatus.Confirmed);

            var totalRevenue = await _context.Bookings
                .SumAsync(b => (decimal?)b.TotalPrice) ?? 0;

            var topTrips = await _context.Bookings
                .Include(b => b.Trip)
                .GroupBy(b => b.Trip.Title)
                .Select(g => new { Trip = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            var recentBookings = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Trip)
                .OrderByDescending(b => b.BookingDate)
                .Take(5)
                .ToListAsync();

            var revenueByMonth = (await _context.Bookings
                .GroupBy(b => new { b.BookingDate.Year, b.BookingDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(x => x.TotalPrice)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync())
                .Select(x => new
                {
                    Month = $"{x.Month}/{x.Year}",
                    Revenue = x.Revenue
                })
                .ToList();

            ViewBag.RevenueByMonth = revenueByMonth;


            ViewBag.TotalTrips = totalTrips;
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalBookings = totalBookings;
            ViewBag.ActiveBookings = activeBookings;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TopTrips = topTrips;
            ViewBag.RecentBookings = recentBookings;
            ViewBag.RevenueByMonth = revenueByMonth;

            return View();
        }
    }
}
