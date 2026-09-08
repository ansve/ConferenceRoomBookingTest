using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Interfaces;

public interface IConferenceRoomRepository
{
    Task<List<ConferenceRoom>> GetAllAsync();
    Task<List<Service>> GetServicesByIdsAsync(List<Guid> serviceIds);
    Task<ConferenceRoom?> GetByIdAsync(Guid id);
    Task AddAsync(ConferenceRoom room);
    Task SaveChangesAsync();
    Task DeleteAsync(ConferenceRoom room);

}
