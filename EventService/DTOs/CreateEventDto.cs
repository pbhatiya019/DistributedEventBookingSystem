using System.ComponentModel.DataAnnotations;

namespace EventService.DTOs
{
    public class CreateEventDto
    {
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
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }
    }
}