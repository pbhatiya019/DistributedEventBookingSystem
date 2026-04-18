using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketService.Data;
using TicketService.DTOs;
using TicketService.Models;
using TicketService.Services;

namespace TicketService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly TicketDbContext _context;
        private readonly EventApiService _eventApiService;

        public TicketsController(TicketDbContext context, EventApiService eventApiService)
        {
            _context = context;
            _eventApiService = eventApiService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetAll()
        {
            var tickets = await _context.Tickets
                .Select(t => new TicketResponseDto
                {
                    Id = t.Id,
                    EventId = t.EventId,
                    AttendeeName = t.AttendeeName,
                    AttendeeEmail = t.AttendeeEmail,
                    BookedByUserId = t.BookedByUserId,
                    BookedAtUtc = t.BookedAtUtc
                })
                .ToListAsync();

            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketResponseDto>> GetById(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound(new { message = $"Ticket with id {id} was not found." });
            }

            var response = new TicketResponseDto
            {
                Id = ticket.Id,
                EventId = ticket.EventId,
                AttendeeName = ticket.AttendeeName,
                AttendeeEmail = ticket.AttendeeEmail,
                BookedByUserId = ticket.BookedByUserId,
                BookedAtUtc = ticket.BookedAtUtc
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TicketResponseDto>> Create(CreateTicketDto dto)
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

            var eventExists = await _eventApiService.EventExistsAsync(dto.EventId);
            if (!eventExists)
            {
                return BadRequest(new { message = $"Cannot create ticket. Event with id {dto.EventId} does not exist." });
            }

            var ticket = new TicketItem
            {
                EventId = dto.EventId,
                AttendeeName = dto.AttendeeName,
                AttendeeEmail = dto.AttendeeEmail,
                BookedByUserId = userId,
                BookedAtUtc = DateTime.UtcNow
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            var response = new TicketResponseDto
            {
                Id = ticket.Id,
                EventId = ticket.EventId,
                AttendeeName = ticket.AttendeeName,
                AttendeeEmail = ticket.AttendeeEmail,
                BookedByUserId = ticket.BookedByUserId,
                BookedAtUtc = ticket.BookedAtUtc
            };

            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, response);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<TicketResponseDto>> Update(int id, UpdateTicketDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound(new { message = $"Ticket with id {id} was not found." });
            }

            ticket.AttendeeName = dto.AttendeeName;
            ticket.AttendeeEmail = dto.AttendeeEmail;

            await _context.SaveChangesAsync();

            var response = new TicketResponseDto
            {
                Id = ticket.Id,
                EventId = ticket.EventId,
                AttendeeName = ticket.AttendeeName,
                AttendeeEmail = ticket.AttendeeEmail,
                BookedByUserId = ticket.BookedByUserId,
                BookedAtUtc = ticket.BookedAtUtc
            };

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound(new { message = $"Ticket with id {id} was not found." });
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Ticket with id {id} deleted successfully." });
        }
    }
}