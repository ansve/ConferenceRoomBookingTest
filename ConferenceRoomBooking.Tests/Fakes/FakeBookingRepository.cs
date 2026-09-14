using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Tests.Fakes;

public class FakeBookingRepository : IBookingRepository
{
    public List<Booking> Bookings { get; } = new();

    public Task<List<Booking>> GetByRoomIdAsync(Guid roomId)
    {
        var bookings = Bookings
            .Where(booking => booking.ConferenceRoomId == roomId)
            .ToList();

        return Task.FromResult(bookings);
    }

    public Task<bool> HasOverlapAsync(
        Guid roomId,
        DateTime startTime,
        DateTime endTime)
    {
        var hasOverlap = Bookings.Any(booking =>
            booking.ConferenceRoomId == roomId &&
            booking.StartTime < endTime &&
            booking.EndTime > startTime);

        return Task.FromResult(hasOverlap);
    }

    public Task AddAsync(Booking booking)
    {
        Bookings.Add(booking);

        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}