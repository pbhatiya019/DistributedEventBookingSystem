using System.ComponentModel.DataAnnotations;

namespace EventService.Models
{
    public class EventItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }
    }
}