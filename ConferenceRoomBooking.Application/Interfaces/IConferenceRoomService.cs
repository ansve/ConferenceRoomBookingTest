using ConferenceRoomBooking.Application.DTOs;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IConferenceRoomService
{
    Task<List<ConferenceRoomResponse>> GetAllAsync();
    Task<ConferenceRoomResponse?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateConferenceRoomRequest request);
    Task UpdateAsync(Guid id, UpdateConferenceRoomRequest request);
    Task DeleteAsync(Guid id);
    Task<List<ConferenceRoomResponse>> SearchAvailableAsync(SearchAvailableRoomsRequest request);
}
