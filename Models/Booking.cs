using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelWebApp.Models
{
    public enum BookingStatus
    {
        Pending = 1,
        Confirmed = 2,
        Cancelled = 3
    }

    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ValidateNever]
        public Customer? Customer { get; set; }

        [Required]
        public int TripId { get; set; }

        [ValidateNever]
        public Trip? Trip { get; set; }

        [Range(1, 50)]
        public int NumberOfPassengers { get; set; }

        public DateTime BookingDate { get; set; }

        public decimal TotalPrice { get; set; }

        [Required]
        public BookingStatus Status { get; set; }
    }
}
