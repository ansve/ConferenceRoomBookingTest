using ConferenceRoomBooking.Application.DTOs;
using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Mappers;

public static class ConferenceRoomMapper
{
    public static ConferenceRoomResponse ToResponse(ConferenceRoom room)
    {
        return new ConferenceRoomResponse
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            BasePricePerHour = room.BasePricePerHour,
            Services = room.Services
                .Select(service => new ServiceResponse
                {
                    Id = service.Id,
                    Name = service.Name,
                    Price = service.Price
                })
                .ToList()
        };
    }
}