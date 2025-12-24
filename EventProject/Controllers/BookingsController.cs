using EventProject.ApplicationDbContext;
using EventProject.Models;
using EventProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventProject.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly EventDbContext _context;
        private readonly MemberbaseService _memberbase;

        public BookingsController(EventDbContext context, MemberbaseService memberbase)
        {
            _context = context;
            _memberbase = memberbase;
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventBooking booking)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors in the form.";
                return RedirectToAction("Details", "Events", new { id = booking.EventId });
            }

            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == booking.EventId);

            if (ev == null)
            {
                TempData["Error"] = "Event not found.";
                return RedirectToAction("Index", "Events");
            }

            var currentBookings = await _context.EventBookings
                .CountAsync(b => b.EventId == booking.EventId && b.Status == "Confirmed");

            if (currentBookings >= ev.Capacity)
            {
                TempData["Error"] = "Sorry, this event is fully booked.";
                return RedirectToAction("Details", "Events", new { id = booking.EventId });
            }

            booking.DateBooked = DateTime.UtcNow;
            booking.Status = "Confirmed";

            _context.EventBookings.Add(booking);
            await _context.SaveChangesAsync();

            var result = await _memberbase.CreateOrGetContact(
                booking.Name,
                booking.Email
            );

            if (!result.Success)
                TempData["Error"] = "Booking saved, but CRM sync failed.";
            else
                TempData["Success"] = "Your booking has been confirmed!";

            return RedirectToAction("Confirmation");
        }


        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
