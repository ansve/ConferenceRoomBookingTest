using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IBookingRepository
{
    Task<List<Booking>> GetByRoomIdAsync(Guid roomId);

    Task AddAsync(Booking booking);

    Task SaveChangesAsync();
    Task<bool> HasOverlapAsync(Guid roomId, DateTime startTime, DateTime endTime);
}