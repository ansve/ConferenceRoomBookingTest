using ConferenceRoomBooking.Application.DTOs;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(CreateBookingRequest request);
}