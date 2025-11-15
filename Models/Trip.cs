using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelWebApp.Models
{
    public enum TransportType
    {
        Flight = 1,
        Bus = 2
    }

    public class Trip
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; } = null!;

        [Required]
        public int DestinationId { get; set; }

        [ValidateNever]
        public Destination? Destination { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        [Required]
        public TransportType TransportType { get; set; }

        [Range(typeof(decimal), "0", "9999999999")]
        public decimal PricePerPerson { get; set; }

        [Range(1, 500)]
        public int TotalSeats { get; set; }

        [Range(0, 500)]
        public int AvailableSeats { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
