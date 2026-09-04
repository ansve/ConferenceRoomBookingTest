using ConferenceRoomBooking.Application.DTOs;
using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IConferenceRoomService
{
    Task<List<ConferenceRoom>> GetAllAsync();
    Task<Guid> CreateAsync(CreateConferenceRoomRequest request);
}
