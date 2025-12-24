using EventProject.Models;

namespace EventProject.Services.Interfaces
{
    public interface IBookingService
    {
        Task<bool> Create(EventBookingRequestModel model);
        Task<Event> GetEventById(int eventId);
        Task<int> GetNumberOfConfirmedBooking(int eventId);
        Task<IEnumerable<Event>> GetEvents();
    }
}
