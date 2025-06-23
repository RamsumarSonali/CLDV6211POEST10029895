using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class EventType
    {
        public int EventTypeId { get; set; }

        [Required(ErrorMessage = "Event type name is required.")]
        [StringLength(100, ErrorMessage = "Event type name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public List<Event> Events { get; set; } = new();
    }
}
