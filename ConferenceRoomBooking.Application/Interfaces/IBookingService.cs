using ConferenceRoomBooking.Application.DTOs;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IBookingService
{
    Task<Guid> CreateAsync(CreateBookingRequest request);
}