using Microsoft.EntityFrameworkCore;
using Presentation.Data;

namespace BookingService.Data;

public class BookingDbContext(DbContextOptions<BookingDbContext> options) : DbContext(options)
{
    public DbSet<BookingEntity> Bookings { get; set; }
}