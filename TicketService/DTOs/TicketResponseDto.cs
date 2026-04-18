namespace TicketService.DTOs
{
    public class TicketResponseDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string AttendeeName { get; set; } = string.Empty;
        public string AttendeeEmail { get; set; } = string.Empty;
        public int BookedByUserId { get; set; }
        public DateTime BookedAtUtc { get; set; }
    }
}