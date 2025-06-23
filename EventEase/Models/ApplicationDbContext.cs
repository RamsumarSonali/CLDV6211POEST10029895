using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace EventEase.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Event { get; set; }
        public DbSet<Venue> Venue { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<EventType> EventType { get; set; } // ✅ Add this line
       // public DbSet<Image> Image { get; set; }         // ✅ Optional: only if Image is used
    }
}
