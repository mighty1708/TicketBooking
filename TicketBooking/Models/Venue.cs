using System.ComponentModel.DataAnnotations;

namespace TicketBooking.Models
{
    public class Venue
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
