using System.ComponentModel.DataAnnotations;
namespace TravelWebApp.Models;

public class Customer
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string FullName { get; set; } = null!;

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Phone]
    public string? Phone { get; set; }

    [StringLength(20)]
    public string? PassportNumber { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
