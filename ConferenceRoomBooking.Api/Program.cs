using ConferenceRoomBooking.Infrastructure.Persistence;
using ConferenceRoomBooking.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ConferenceRoomBookingDb"));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

await SeedDatabaseAsync(app);

app.Run();

static async Task SeedDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await DatabaseSeeder.SeedAsync(context);
}