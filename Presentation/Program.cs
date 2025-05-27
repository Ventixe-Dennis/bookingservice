using BookingService.Business;
using BookingService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<BookingDbContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("BookingDatabaseConnection")));

builder.Services.AddScoped<IBookingService, BookingHandler>();

var app = builder.Build();



app.MapOpenApi();


app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
