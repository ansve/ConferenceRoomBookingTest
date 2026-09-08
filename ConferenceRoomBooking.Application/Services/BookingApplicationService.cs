using ConferenceRoomBooking.Application.DTOs;
using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Domain.Exceptions;

namespace ConferenceRoomBooking.Application.Services;

public class BookingApplicationService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IConferenceRoomRepository _conferenceRoomRepository;

    public BookingApplicationService(IBookingRepository bookingRepository,
        IConferenceRoomRepository conferenceRoomRepository)
    {
        _bookingRepository = bookingRepository;
        _conferenceRoomRepository = conferenceRoomRepository;
    }

    public async Task<Guid> CreateAsync(CreateBookingRequest request)
    {
        var room = await _conferenceRoomRepository.GetByIdAsync(request.ConferenceRoomId);

        if (room is null)
        {
            throw new ConferenceRoomNotFoundException(request.ConferenceRoomId);
        }

        var hasOverlap = await _bookingRepository.HasOverlapAsync(
            request.ConferenceRoomId,
            request.StartTime,
            request.EndTime);

        if (hasOverlap)
        {
            throw new BusinessException("Conference room is already booked for the selected period.");
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

        var booking = new Booking(
            request.ConferenceRoomId,
            request.StartTime,
            request.EndTime,
            room.BasePricePerHour);

        foreach (var service in services)
        {
            var bookingService = new BookingService(
                booking.Id,
                service.Id,
                service.Price);

            booking.BookingServices.Add(bookingService);
        }

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        return booking.Id;
    }
}