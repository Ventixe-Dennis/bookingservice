using Microsoft.AspNetCore.Mvc;
using BookingService.Business;
using Presentation.Data;

namespace BookingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;

    [HttpPost]
    public async Task<IActionResult> Create(BookingEntity entity)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bookingService.CreateAsync(entity);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }
}