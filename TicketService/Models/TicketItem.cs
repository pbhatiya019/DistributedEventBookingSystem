using System.ComponentModel.DataAnnotations;

namespace TicketService.Models
{
    public class TicketItem
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        [MaxLength(150)]
        public string AttendeeName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string AttendeeEmail { get; set; } = string.Empty;

        [Required]
        public int BookedByUserId { get; set; }

        public DateTime BookedAtUtc { get; set; } = DateTime.UtcNow;
    }
}