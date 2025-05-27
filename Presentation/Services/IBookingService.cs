using Presentation.Data;

namespace BookingService.Business
{
    public interface IBookingService
    {
        Task<BookingEntity> CreateAsync(BookingEntity entity);
    }
}