using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IConferenceRoomRepository
{
    Task<List<ConferenceRoom>> GetAllAsync();
    Task<List<Service>> GetServicesByIdsAsync(List<Guid> serviceIds);
    Task AddAsync(ConferenceRoom room);
}
