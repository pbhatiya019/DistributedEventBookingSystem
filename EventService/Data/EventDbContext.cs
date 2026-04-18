using EventService.Models;
using Microsoft.EntityFrameworkCore;

namespace EventService.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
        }

        public DbSet<EventItem> Events => Set<EventItem>();
    }
}