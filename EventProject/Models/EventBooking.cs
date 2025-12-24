using System.ComponentModel.DataAnnotations;

namespace EventProject.Models
{
    public class EventBooking
    {
        public int Id { get; set; }
        public int EventId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Comment { get; set; }

        public DateTime DateBooked { get; set; }
        public string Status { get; set; }

        public Event Event { get; set; }
    }

    public class EventBookingRequestModel
    {
        public int EventId { get; set; }
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Comment { get; set; }
    }
}
