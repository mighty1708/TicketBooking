using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Models;

namespace TicketBooking.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Event entity
            builder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Venue).WithMany(v => v.Events).HasForeignKey(e => e.VenueId);
            });

            // Configure Venue entity
            builder.Entity<Venue>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Name).IsRequired().HasMaxLength(200);
                entity.Property(v => v.Address).IsRequired().HasMaxLength(500);
                entity.Property(v => v.Capacity).IsRequired();
            });

            // Configure Seat entity
            builder.Entity<Seat>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.SeatNumber).IsRequired().HasMaxLength(10);
                entity.Property(s => s.RowNumber).IsRequired().HasMaxLength(10);
                entity.Property(s => s.Price).HasColumnType("decimal(18,2)");
                entity.HasOne(s => s.Venue).WithMany(v => v.Seats).HasForeignKey(s => s.VenueId);
            });

            // Configure Booking entity
            builder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(b => b.BookingDate).IsRequired();
                entity.HasOne(b => b.User).WithMany(u => u.Bookings).HasForeignKey(b => b.UserId);
                entity.HasOne(b => b.Event).WithMany(e => e.Bookings).HasForeignKey(b => b.EventId);
            });

            // Configure Ticket entity
            builder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.TicketNumber).IsRequired().HasMaxLength(50);
                entity.Property(t => t.Price).HasColumnType("decimal(18,2)");
                entity.HasOne(t => t.Booking).WithMany(b => b.Tickets).HasForeignKey(t => t.BookingId);
                entity.HasOne(t => t.Seat).WithMany(s => s.Tickets).HasForeignKey(t => t.SeatId);
            });

            // Configure Payment entity
            builder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Amount).HasColumnType("decimal(18,2)");
                entity.Property(p => p.PaymentDate).IsRequired();
                entity.Property(p => p.StripePaymentIntentId).HasMaxLength(100);
                entity.HasOne(p => p.Booking).WithOne(b => b.Payment).HasForeignKey<Payment>(p => p.BookingId);
            });
        }
    }
}
