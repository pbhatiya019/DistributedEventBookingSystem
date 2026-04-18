namespace ClientApp.Models
{
    public class CreateTicketRequest
    {
        public int EventId { get; set; }
        public string AttendeeName { get; set; } = string.Empty;
        public string AttendeeEmail { get; set; } = string.Empty;
    }
}