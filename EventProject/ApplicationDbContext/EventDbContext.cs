using EventProject.Models;
using Microsoft.EntityFrameworkCore;

namespace EventProject.ApplicationDbContext
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options)
        : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventBooking> EventBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Fluent API for table configuration
            modelBuilder.Entity<Event>().ToTable("Events");
            modelBuilder.Entity<EventBooking>().ToTable("EventBookings");
        }
    }
}
