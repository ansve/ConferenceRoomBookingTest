using ConferenceRoomBooking.Application.DTOs;
using ConferenceRoomBooking.Application.Services;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Domain.Exceptions;
using ConferenceRoomBooking.Tests.Fakes;

namespace ConferenceRoomBooking.Tests;

public class BookingApplicationServiceTests
{
    [Fact]
    public async Task CreateAsync_WhenRoomDoesNotExist_ThrowsConferenceRoomNotFoundException()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();

        var bookingService = new BookingApplicationService(
            new FakeBookingRepository(),
            conferenceRoomRepository,
            new BookingPriceCalculator());

        var request = new CreateBookingRequest
        {
            ConferenceRoomId = Guid.NewGuid(),
            StartTime = new DateTime(2026, 9, 10, 10, 0, 0),
            EndTime = new DateTime(2026, 9, 10, 12, 0, 0)
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConferenceRoomNotFoundException>(
            () => bookingService.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_WhenRoomIsAlreadyBooked_ThrowsRoomAlreadyBookedException()
    {
        // Arrange
        var bookingRepository = new FakeBookingRepository();
        var conferenceRoomRepository = new FakeConferenceRoomRepository();

        var bookingService = new BookingApplicationService(
            bookingRepository,
            conferenceRoomRepository,
            new BookingPriceCalculator());

        var room = new ConferenceRoom(
            "Зал A",
            50,
            2000m);

        conferenceRoomRepository.Rooms.Add(room);

        var existingBooking = new Booking(
            room.Id,
            new DateTime(2026, 9, 10, 10, 0, 0),
            new DateTime(2026, 9, 10, 14, 0, 0),
            room.BasePricePerHour,
            8000m);

        bookingRepository.Bookings.Add(existingBooking);

        var request = new CreateBookingRequest
        {
            ConferenceRoomId = room.Id,
            StartTime = new DateTime(2026, 9, 10, 12, 0, 0),
            EndTime = new DateTime(2026, 9, 10, 13, 0, 0)
        };

        // Act & Assert
        await Assert.ThrowsAsync<RoomAlreadyBookedException>(
            () => bookingService.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_WhenBookingIsValid_CreatesBookingAndReturnsCorrectPrice()
    {
        // Arrange
        var bookingRepository = new FakeBookingRepository();
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var priceCalculator = new BookingPriceCalculator();

        var bookingService = new BookingApplicationService(
            bookingRepository,
            conferenceRoomRepository,
            priceCalculator);

        var room = new ConferenceRoom(
            "Зал A",
            50,
            2000m);

        var projector = new Service(
            "Проектор",
            500m);

        conferenceRoomRepository.Rooms.Add(room);
        conferenceRoomRepository.Services.Add(projector);

        room.Services.Add(projector);

        var request = new CreateBookingRequest
        {
            ConferenceRoomId = room.Id,
            StartTime = new DateTime(2026, 9, 10, 10, 0, 0),
            EndTime = new DateTime(2026, 9, 10, 12, 0, 0),
            ServiceIds = new List<Guid>
        {
            projector.Id
        }
        };

        // Act
        var result = await bookingService.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(room.Id, result.ConferenceRoomId);
        Assert.Equal(request.StartTime, result.StartTime);
        Assert.Equal(request.EndTime, result.EndTime);

        Assert.Equal(4000m, result.RoomPrice);
        Assert.Equal(500m, result.ServicesPrice);
        Assert.Equal(4500m, result.TotalPrice);

        Assert.Single(bookingRepository.Bookings);

        var savedBooking = bookingRepository.Bookings[0];

        Assert.Equal(result.Id, savedBooking.Id);
        Assert.Equal(room.Id, savedBooking.ConferenceRoomId);
        Assert.Equal(2000m, savedBooking.BasePricePerHour);
        Assert.Equal(4500m, savedBooking.TotalPrice);

        Assert.Single(savedBooking.BookingServices);

        var bookingServiceEntity = savedBooking.BookingServices.First();

        Assert.Equal(projector.Id, bookingServiceEntity.ServiceId);
        Assert.Equal(500m, bookingServiceEntity.Price);
    }

    [Fact]
    public async Task CreateAsync_WhenNewBookingStartsWhenExistingBookingEnds_CreatesBooking()
    {
        // Arrange
        var bookingRepository = new FakeBookingRepository();
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var priceCalculator = new BookingPriceCalculator();

        var bookingService = new BookingApplicationService(
            bookingRepository,
            conferenceRoomRepository,
            priceCalculator);

        var room = new ConferenceRoom(
            "Зал A",
            50,
            2000m);

        conferenceRoomRepository.Rooms.Add(room);

        var existingBooking = new Booking(
            room.Id,
            new DateTime(2026, 9, 10, 10, 0, 0),
            new DateTime(2026, 9, 10, 14, 0, 0),
            room.BasePricePerHour,
            8000m);

        bookingRepository.Bookings.Add(existingBooking);

        var request = new CreateBookingRequest
        {
            ConferenceRoomId = room.Id,
            StartTime = new DateTime(2026, 9, 10, 14, 0, 0),
            EndTime = new DateTime(2026, 9, 10, 16, 0, 0)
        };

        // Act
        var result = await bookingService.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(room.Id, result.ConferenceRoomId);
        Assert.Equal(4000m, result.RoomPrice);
        Assert.Equal(4000m, result.TotalPrice);

        Assert.Equal(2, bookingRepository.Bookings.Count);
    }

    [Fact]
    public async Task CreateAsync_WhenNewBookingEndsWhenExistingBookingStarts_CreatesBooking()
    {
        // Arrange
        var bookingRepository = new FakeBookingRepository();
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var priceCalculator = new BookingPriceCalculator();

        var bookingService = new BookingApplicationService(
            bookingRepository,
            conferenceRoomRepository,
            priceCalculator);

        var room = new ConferenceRoom(
            "Зал A",
            50,
            2000m);

        conferenceRoomRepository.Rooms.Add(room);

        var existingBooking = new Booking(
            room.Id,
            new DateTime(2026, 9, 10, 10, 0, 0),
            new DateTime(2026, 9, 10, 14, 0, 0),
            room.BasePricePerHour,
            8000m);

        bookingRepository.Bookings.Add(existingBooking);

        var request = new CreateBookingRequest
        {
            ConferenceRoomId = room.Id,
            StartTime = new DateTime(2026, 9, 10, 8, 0, 0),
            EndTime = new DateTime(2026, 9, 10, 10, 0, 0)
        };

        // Act
        var result = await bookingService.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(room.Id, result.ConferenceRoomId);
        Assert.Equal(3800m, result.RoomPrice);
        Assert.Equal(3800m, result.TotalPrice);

        Assert.Equal(2, bookingRepository.Bookings.Count);
    }

    [Fact]
    public async Task CreateAsync_WhenServiceDoesNotExist_ThrowsServiceNotFoundException()
    {
        // Arrange
        var bookingRepository = new FakeBookingRepository();
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var priceCalculator = new BookingPriceCalculator();

        var bookingService = new BookingApplicationService(
            bookingRepository,
            conferenceRoomRepository,
            priceCalculator);

        var room = new ConferenceRoom(
            "Зал A",
            50,
            2000m);

        conferenceRoomRepository.Rooms.Add(room);

        var request = new CreateBookingRequest
        {
            ConferenceRoomId = room.Id,
            StartTime = new DateTime(2026, 9, 10, 10, 0, 0),
            EndTime = new DateTime(2026, 9, 10, 12, 0, 0),
            ServiceIds = new List<Guid>
        {
            Guid.NewGuid()
        }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ServiceNotFoundException>(
            () => bookingService.CreateAsync(request));
    }
}