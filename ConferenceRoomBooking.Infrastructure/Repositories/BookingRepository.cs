using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Booking>> GetByRoomIdAsync(Guid roomId)
    {
        return await _context.Bookings
            .Where(booking => booking.ConferenceRoomId == roomId)
            .ToListAsync();
    }

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasOverlapAsync(Guid roomId, DateTime startTime, DateTime endTime)
    {
        return await _context.Bookings
            .AnyAsync(booking =>
                booking.ConferenceRoomId == roomId &&
                booking.StartTime < endTime &&
                booking.EndTime > startTime);
    }
}