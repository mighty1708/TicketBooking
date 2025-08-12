using TicketBooking.Models;

namespace TicketBooking.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            if (context.Venues.Any())
                return;

            // Seed Venues
            var venues = new List<Venue>
            {
                new Venue
                {
                    Name = "Grand Concert Hall",
                    Address = "123 Music Street, Downtown",
                    Capacity = 1000,
                    Description = "A beautiful concert hall with excellent acoustics"
                },
                new Venue
                {
                    Name = "Sports Arena",
                    Address = "456 Stadium Road, Sports District",
                    Capacity = 5000,
                    Description = "Large arena perfect for sports events and concerts"
                },
                new Venue
                {
                    Name = "Theater Royal",
                    Address = "789 Arts Avenue, Cultural Quarter",
                    Capacity = 500,
                    Description = "Intimate theater for plays and performances"
                }
            };

            context.Venues.AddRange(venues);
            await context.SaveChangesAsync();

            // Seed Events
            var events = new List<Event>
            {
                new Event
                {
                    Title = "Classical Symphony Night",
                    Description = "Experience the magic of classical music with the city's finest orchestra.",
                    Date = DateTime.Now.AddDays(30),
                    Time = new TimeSpan(19, 30, 0),
                    Price = 75.00m,
                    VenueId = venues[0].Id,
                    Category = "Music",
                    ImageUrl = "/images/classical-concert.jpg"
                },
                new Event
                {
                    Title = "Rock Concert Extravaganza",
                    Description = "The biggest rock concert of the year featuring top artists.",
                    Date = DateTime.Now.AddDays(45),
                    Time = new TimeSpan(20, 0, 0),
                    Price = 120.00m,
                    VenueId = venues[1].Id,
                    Category = "Music",
                    ImageUrl = "/images/rock-concert.jpg"
                },
                new Event
                {
                    Title = "Shakespeare's Hamlet",
                    Description = "A modern interpretation of the classic tragedy.",
                    Date = DateTime.Now.AddDays(15),
                    Time = new TimeSpan(19, 0, 0),
                    Price = 45.00m,
                    VenueId = venues[2].Id,
                    Category = "Theater",
                    ImageUrl = "/images/hamlet.jpg"
                },
                new Event
                {
                    Title = "Basketball Championship",
                    Description = "The final game of the season championship.",
                    Date = DateTime.Now.AddDays(20),
                    Time = new TimeSpan(18, 30, 0),
                    Price = 85.00m,
                    VenueId = venues[1].Id,
                    Category = "Sports",
                    ImageUrl = "/images/basketball.jpg"
                }
            };

            context.Events.AddRange(events);
            await context.SaveChangesAsync();

            // Seed Seats for each venue
            foreach (var venue in venues)
            {
                var seats = new List<Seat>();
                
                // Generate seats based on venue capacity
                int rows = venue.Capacity switch
                {
                    <= 500 => 10,
                    <= 1000 => 20,
                    _ => 25
                };
                
                int seatsPerRow = venue.Capacity / rows;

                for (int row = 1; row <= rows; row++)
                {
                    for (int seat = 1; seat <= seatsPerRow; seat++)
                    {
                        var basePrice = venue.Capacity switch
                        {
                            <= 500 => 45.00m,
                            <= 1000 => 75.00m,
                            _ => 120.00m
                        };

                        // Premium pricing for front rows
                        var priceMultiplier = row <= 3 ? 1.5m : row <= 10 ? 1.2m : 1.0m;
                        
                        seats.Add(new Seat
                        {
                            RowNumber = row.ToString(),
                            SeatNumber = seat.ToString(),
                            Price = basePrice * priceMultiplier,
                            VenueId = venue.Id,
                            IsAvailable = true
                        });
                    }
                }

                context.Seats.AddRange(seats);
            }

            await context.SaveChangesAsync();
        }
    }
}
