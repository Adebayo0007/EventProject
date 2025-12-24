using EventProject.ApplicationDbContext;
using EventProject.Models;
using EventProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventProject.Services
{
    public class BookingService : IBookingService
    {
        private readonly EventDbContext _context;
        public BookingService(EventDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(EventBookingRequestModel model)
        {
            try
            {
                if(model != null)
                {
                    var booking = new EventBooking();
                    booking.EventId = model.EventId;
                    booking.Name = model.Name;
                    booking.Email = model.Email;
                    booking.Comment = model.Comment;
                    booking.DateBooked = DateTime.UtcNow;
                    booking.Status = "Confirmed";

                    var response = await _context.EventBookings.AddAsync(booking);
                    await _context.SaveChangesAsync();
                    if(response.Entity != null) return true;
                    return false;
                }
                return false;
            }
            catch(Exception ex) { Console.WriteLine(ex.StackTrace); return false; }

        }

        public async Task<Event> GetEventById(int eventId) => await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId);

        public async Task<IEnumerable<Event>> GetEvents() => _context.Events.AsQueryable();

        public async Task<int> GetNumberOfConfirmedBooking(int eventId) => await _context.EventBookings
                .CountAsync(b => b.EventId == eventId && b.Status == "Confirmed");

    }
}
