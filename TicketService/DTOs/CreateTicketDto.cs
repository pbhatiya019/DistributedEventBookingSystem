using System.ComponentModel.DataAnnotations;

namespace TicketService.DTOs
{
    public class CreateTicketDto
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        [MaxLength(150)]
        public string AttendeeName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string AttendeeEmail { get; set; } = string.Empty;
    }
}