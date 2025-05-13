using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase.Models
{
    public class Venue
    {
        public int VenueId { get; set; }
        public string? VenueName { get; set; }
        public string? Location { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a greater than 0.")]
        public int Capacity { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public string? ImageUrl { get; set; }
        //add this new one - only for uploading from the Create/Edit form
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

       
        public List<Event>? Events { get; set; } = new();

    }
}
