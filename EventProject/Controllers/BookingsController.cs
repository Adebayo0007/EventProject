using EventProject.ApplicationDbContext;
using EventProject.Models;
using EventProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            booking.DateBooked = DateTime.UtcNow;
            booking.Status = "Confirmed";

            _context.EventBookings.Add(booking);
            await _context.SaveChangesAsync();

            var synced = await _memberbase.CreateOrGetContact(booking.Name, booking.Email);

            if (!synced)
                TempData["Error"] = "Booking saved but CRM sync failed.";
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
