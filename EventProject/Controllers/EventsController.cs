using EventProject.ApplicationDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventProject.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventDbContext _context;

        public EventsController(EventDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? date)
        {
            var events = _context.Events.AsQueryable();

            if (date.HasValue)
                events = events.Where(e => e.EventDateTime.Date == date.Value.Date);

            return View(await events.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var ev = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (ev == null) return NotFound();
            return View(ev);
        }
    }
}
