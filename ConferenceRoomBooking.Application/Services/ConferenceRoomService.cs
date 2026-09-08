using ConferenceRoomBooking.Application.DTOs;
using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Application.Mappers;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Domain.Exceptions;



namespace ConferenceRoomBooking.Application.Services;

public class ConferenceRoomService : IConferenceRoomService
{
    private readonly IConferenceRoomRepository _conferenceRoomRepository;
    private readonly IBookingRepository _bookingRepository;

    public ConferenceRoomService(IConferenceRoomRepository conferenceRoomRepository, IBookingRepository bookingRepository)
    {
        _conferenceRoomRepository = conferenceRoomRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<List<ConferenceRoomResponse>> GetAllAsync()
    {
        var rooms = await _conferenceRoomRepository.GetAllAsync();            

        return rooms.Select(ConferenceRoomMapper.ToResponse).ToList();
    }

    public async Task<ConferenceRoomResponse?> GetByIdAsync(Guid id)
    {
        var room =  await _conferenceRoomRepository.GetByIdAsync(id);

        return room is null ? null : ConferenceRoomMapper.ToResponse(room);
    }

    public async Task<Guid> CreateAsync(CreateConferenceRoomRequest request)
    {
        var services = await _conferenceRoomRepository.GetServicesByIdsAsync(request.ServiceIds);

        var foundServiceIds = services
            .Select(service => service.Id)
            .ToHashSet();

        var missingServiceId = request.ServiceIds
            .FirstOrDefault(serviceId => !foundServiceIds.Contains(serviceId));

        if (missingServiceId != Guid.Empty)
        {
            throw new ServiceNotFoundException(missingServiceId);
        }

        var room = new ConferenceRoom(
            request.Name,
            request.Capacity,
            request.BasePricePerHour);

        foreach (var service in services)
        {
            room.Services.Add(service);
        }

        await _conferenceRoomRepository.AddAsync(room);

        return room.Id;
    }

    public async Task UpdateAsync(Guid id,UpdateConferenceRoomRequest request)
    {
        var room = await _conferenceRoomRepository.GetByIdAsync(id);

        if (room is null)
        {
            throw new ConferenceRoomNotFoundException(id);
        }

        var services = await _conferenceRoomRepository.GetServicesByIdsAsync(request.ServiceIds);

        var foundServiceIds = services
            .Select(service => service.Id)
            .ToHashSet();

        var missingServiceId = request.ServiceIds.FirstOrDefault(serviceId => !foundServiceIds.Contains(serviceId));

        if (missingServiceId != Guid.Empty)
        {
            throw new ServiceNotFoundException(missingServiceId);
        }

        room.Update(
            request.Name,
            request.Capacity,
            request.BasePricePerHour);

        room.Services.Clear();

        foreach (var service in services)
        {
            room.Services.Add(service);
        }

        await _conferenceRoomRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var room = await _conferenceRoomRepository.GetByIdAsync(id);

        if (room is null)
        {
            throw new ConferenceRoomNotFoundException(id);
        }

        await _conferenceRoomRepository.DeleteAsync(room);
    }

    public async Task<List<ConferenceRoomResponse>> SearchAvailableAsync(SearchAvailableRoomsRequest request)
    {
        if (request.StartTime >= request.EndTime)
        {
            throw new InvalidBookingPeriodException(request.StartTime, request.EndTime);
        }

        var rooms = await _conferenceRoomRepository.GetAllAsync();

        var suitableRooms = rooms
            .Where(room => room.Capacity >= request.Capacity)
            .ToList();

        var availableRooms = new List<ConferenceRoom>();

        foreach (var room in suitableRooms)
        {
            var hasOverlap = await _bookingRepository.HasOverlapAsync(
                room.Id,
                request.StartTime,
                request.EndTime);

            if (!hasOverlap)
            {
                availableRooms.Add(room);
            }
        }

        return availableRooms
            .Select(ConferenceRoomMapper.ToResponse)
            .ToList();
    }
}
