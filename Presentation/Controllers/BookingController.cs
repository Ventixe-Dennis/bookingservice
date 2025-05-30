using Microsoft.AspNetCore.Mvc;
using BookingService.Business;
using Presentation.Data;
using System.Text.Json;
using System.Text;

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

        try
        {
            using var client = new HttpClient();

            var eventRes = await new HttpClient().GetAsync(
                $"https://dennis-eventservice-ggebbngthpcxd6g2.swedencentral-01.azurewebsites.net/api/events/{entity.EventId}"
            );

            string eventName = "okänt event";
            if (eventRes.IsSuccessStatusCode)
            {
                var jsonString = await eventRes.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonString);
                eventName = doc.RootElement.GetProperty("result").GetProperty("name").GetString() ?? eventName;
            }


            var payload = new
            {
                email = entity.Email,
                name = entity.Name,
                eventName,
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var emailRes = await client.PostAsync("https://dennis-emailservice-frffenc6hderehcy.swedencentral-01.azurewebsites.net/api/email/send", content);
            if (!emailRes.IsSuccessStatusCode)
            {
                Console.WriteLine($"EmailService error: {emailRes.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling EmailService: {ex.Message}");
        }



        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }
}