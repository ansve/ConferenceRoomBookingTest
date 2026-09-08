using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Repositories;

public class ConferenceRoomRepository : IConferenceRoomRepository
{
    private readonly AppDbContext _context;

    public ConferenceRoomRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ConferenceRoom>> GetAllAsync()
    {
        return await _context.ConferenceRooms
            .Include(room => room.Services)
            .ToListAsync();
    }

    public async Task<List<Service>> GetServicesByIdsAsync(List<Guid> serviceIds)
    {
        return await _context.Services
            .Where(service => serviceIds.Contains(service.Id))
            .ToListAsync();
    }

    public async Task<ConferenceRoom?> GetByIdAsync(Guid id)
    {
        return await _context.ConferenceRooms
            .Include(room => room.Services)
            .FirstOrDefaultAsync(room => room.Id == id);
    }

    public async Task AddAsync(ConferenceRoom room)
    {
        await _context.ConferenceRooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ConferenceRoom room)
    {
        _context.ConferenceRooms.Remove(room);
        await _context.SaveChangesAsync();
    }
}
