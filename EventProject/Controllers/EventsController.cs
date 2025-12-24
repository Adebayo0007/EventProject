using EventProject.ApplicationDbContext;
using EventProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventProject.Controllers
{
    public class EventsController : Controller
    {
        private readonly IBookingService _bookingService;

        public EventsController(IBookingService bookingService)
        {
           _bookingService = bookingService;
        }

        public async Task<IActionResult> Index(DateTime? date)
        {
            var events = await _bookingService.GetEvents();

            if (date.HasValue)
                events = events.Where(e => e.EventDateTime.Date == date.Value.Date);

            return View(events);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ev = await _bookingService.GetEventById(id);
            if (ev == null) return NotFound();
            return View(ev);
        }
    }
}
