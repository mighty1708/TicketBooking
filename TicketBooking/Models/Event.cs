using System.ComponentModel.DataAnnotations;

namespace TicketBooking.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int VenueId { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        // Navigation properties
        public virtual Venue? Venue { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        // Computed properties
        public DateTime DateTime => Date.Add(Time);
        public bool IsUpcoming => DateTime > DateTime.Now;
        public int AvailableSeats => Venue?.Capacity - Bookings.Sum(b => b.Tickets.Count) ?? 0;
    }
}
