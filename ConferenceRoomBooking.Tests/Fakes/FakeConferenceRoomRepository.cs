using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Tests.Fakes;

public class FakeConferenceRoomRepository : IConferenceRoomRepository
{
    public List<ConferenceRoom> Rooms { get; } = new();
    public List<Service> Services { get; } = new();

    public Task<List<ConferenceRoom>> GetAllAsync()
    {
        return Task.FromResult(Rooms);
    }

    public Task<List<Service>> GetServicesByIdsAsync(List<Guid> serviceIds)
    {
        var services = Services
            .Where(service => serviceIds.Contains(service.Id))
            .ToList();

        return Task.FromResult(services);
    }

    public Task<ConferenceRoom?> GetByIdAsync(Guid id)
    {
        var room = Rooms.FirstOrDefault(room => room.Id == id);

        return Task.FromResult(room);
    }

    public Task AddAsync(ConferenceRoom room)
    {
        Rooms.Add(room);

        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ConferenceRoom room)
    {
        Rooms.Remove(room);

        return Task.CompletedTask;
    }
}