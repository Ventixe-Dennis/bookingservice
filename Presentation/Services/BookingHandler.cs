using BookingService.Data;
using Presentation.Data;

namespace BookingService.Business;

public class BookingHandler(BookingDbContext context) : IBookingService
{
    private readonly BookingDbContext _context = context;

    public async Task<BookingEntity> CreateAsync(BookingEntity entity)
    {
        _context.Bookings.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}