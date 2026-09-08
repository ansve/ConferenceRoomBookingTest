using ConferenceRoomBooking.Application.DTOs;
using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Domain.Exceptions;

namespace ConferenceRoomBooking.Application.Services;

public class BookingApplicationService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IConferenceRoomRepository _conferenceRoomRepository;
    private readonly BookingPriceCalculator _priceCalculator;

    public BookingApplicationService(IBookingRepository bookingRepository,
        IConferenceRoomRepository conferenceRoomRepository,
        BookingPriceCalculator priceCalculator)
    {
        _bookingRepository = bookingRepository;
        _conferenceRoomRepository = conferenceRoomRepository;
        _priceCalculator = priceCalculator;
    }

    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
    {
        if (request.StartTime >= request.EndTime)
        {
            throw new InvalidBookingPeriodException(request.StartTime, request.EndTime);
        }

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
            throw new RoomAlreadyBookedException();
        }

        var roomPrice = _priceCalculator.Calculate(
            request.StartTime,
            request.EndTime,
            room.BasePricePerHour);

        var services = await _conferenceRoomRepository.GetServicesByIdsAsync(request.ServiceIds);

        var servicesPrice = services.Sum(service => service.Price);

        var totalPrice = roomPrice + servicesPrice;

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
            room.BasePricePerHour,
            totalPrice);

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

        return new BookingResponse
        {
            Id = booking.Id,
            ConferenceRoomId = booking.ConferenceRoomId,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            RoomPrice = roomPrice,
            ServicesPrice = servicesPrice,
            TotalPrice = totalPrice
        };
    }
}