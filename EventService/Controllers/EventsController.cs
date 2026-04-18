using System.Security.Claims;
using EventService.Data;
using EventService.DTOs;
using EventService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventDbContext _context;

        public EventsController(EventDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetAll()
        {
            var events = await _context.Events
                .Select(e => new EventResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Location = e.Location,
                    EventDate = e.EventDate,
                    Capacity = e.Capacity,
                    CreatedByUserId = e.CreatedByUserId
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponseDto>> GetById(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound(new { message = $"Event with id {id} was not found." });
            }

            var response = new EventResponseDto
            {
                Id = eventItem.Id,
                Title = eventItem.Title,
                Description = eventItem.Description,
                Location = eventItem.Location,
                EventDate = eventItem.EventDate,
                Capacity = eventItem.Capacity,
                CreatedByUserId = eventItem.CreatedByUserId
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<EventResponseDto>> Create(CreateEventDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst(ClaimTypes.Name)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token. User ID not found." });
            }

            var eventItem = new EventItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                EventDate = dto.EventDate,
                Capacity = dto.Capacity,
                CreatedByUserId = userId
            };

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            var response = new EventResponseDto
            {
                Id = eventItem.Id,
                Title = eventItem.Title,
                Description = eventItem.Description,
                Location = eventItem.Location,
                EventDate = eventItem.EventDate,
                Capacity = eventItem.Capacity,
                CreatedByUserId = eventItem.CreatedByUserId
            };

            return CreatedAtAction(nameof(GetById), new { id = eventItem.Id }, response);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<EventResponseDto>> Update(int id, UpdateEventDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound(new { message = $"Event with id {id} was not found." });
            }

            eventItem.Title = dto.Title;
            eventItem.Description = dto.Description;
            eventItem.Location = dto.Location;
            eventItem.EventDate = dto.EventDate;
            eventItem.Capacity = dto.Capacity;

            await _context.SaveChangesAsync();

            var response = new EventResponseDto
            {
                Id = eventItem.Id,
                Title = eventItem.Title,
                Description = eventItem.Description,
                Location = eventItem.Location,
                EventDate = eventItem.EventDate,
                Capacity = eventItem.Capacity,
                CreatedByUserId = eventItem.CreatedByUserId
            };

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound(new { message = $"Event with id {id} was not found." });
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Event with id {id} deleted successfully." });
        }
    }
}