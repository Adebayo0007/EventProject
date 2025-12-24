using EventProject.ApplicationDbContext;
using EventProject.Models;
using EventProject.Services;
using EventProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventProject.Controllers
{
    //[Authorize]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly MemberbaseService _memberbase;

        public BookingsController(IBookingService bookingService, MemberbaseService memberbase)
        {
            _bookingService = bookingService;
            _memberbase = memberbase;
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventBookingRequestModel bookingModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors in the form.";
                return RedirectToAction("Details", "Events", new { id = bookingModel.EventId });
            }

            var ev = await _bookingService.GetEventById(bookingModel.EventId);

            if (ev == null)
            {
                TempData["Error"] = "Event not found.";
                return RedirectToAction("Index", "Events");
            }

            var currentBookings = await _bookingService.GetNumberOfConfirmedBooking(bookingModel.EventId);

            if (currentBookings >= ev.Capacity)
            {
                TempData["Error"] = "Sorry, this event is fully booked.";
                return RedirectToAction("Details", "Events", new { id = bookingModel.EventId });
            }
            var createResponse = await _bookingService.Create(bookingModel);
            if(createResponse)
            {
                var result = await _memberbase.CreateOrGetContact(
              bookingModel.Name,
              bookingModel.Email
              );

                if (!result.Success)
                    TempData["Error"] = "Booking saved, but CRM sync failed.";
                else
                    TempData["Success"] = "Your booking has been confirmed!";

                return RedirectToAction("Confirmation");

            }
            TempData["Error"] = "Sorry, an error occur when saving your data.";
            return RedirectToAction("Details", "Events", new { id = bookingModel.EventId });


        }


        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
