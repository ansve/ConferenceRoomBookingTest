using ConferenceRoomBooking.Application.DTOs;
using ConferenceRoomBooking.Application.Services;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Domain.Exceptions;
using ConferenceRoomBooking.Tests.Fakes;

namespace ConferenceRoomBooking.Tests;

public class ConferenceRoomServiceTests
{
    [Fact]
    public async Task CreateAsync_WhenRequestIsValid_CreatesRoomWithSelectedServices()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var projector = new Service(
            "Проектор",
            500m);

        var wiFi = new Service(
            "Wi-Fi",
            300m);

        conferenceRoomRepository.Services.Add(projector);
        conferenceRoomRepository.Services.Add(wiFi);

        var request = new CreateConferenceRoomRequest
        {
            Name = "Зал A",
            Capacity = 50,
            BasePricePerHour = 2000m,
            ServiceIds = new List<Guid>
            {
                projector.Id,
                wiFi.Id
            }
        };

        // Act
        var roomId = await conferenceRoomService.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, roomId);

        Assert.Single(conferenceRoomRepository.Rooms);

        var savedRoom = conferenceRoomRepository.Rooms[0];

        Assert.Equal(roomId, savedRoom.Id);
        Assert.Equal("Зал A", savedRoom.Name);
        Assert.Equal(50, savedRoom.Capacity);
        Assert.Equal(2000m, savedRoom.BasePricePerHour);

        Assert.Equal(2, savedRoom.Services.Count);
        Assert.Contains(projector, savedRoom.Services);
        Assert.Contains(wiFi, savedRoom.Services);
    }

    [Fact]
    public async Task CreateAsync_WhenServiceDoesNotExist_ThrowsServiceNotFoundException()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var missingServiceId = Guid.NewGuid();

        var request = new CreateConferenceRoomRequest
        {
            Name = "Зал A",
            Capacity = 50,
            BasePricePerHour = 2000m,
            ServiceIds = new List<Guid>
        {
            missingServiceId
        }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ServiceNotFoundException>(
            () => conferenceRoomService.CreateAsync(request));

        Assert.Empty(conferenceRoomRepository.Rooms);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRoomExists_ReturnsRoom()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var room = new ConferenceRoom(
            "Зал A",
            50,
            2000m);

        var projector = new Service(
            "Проектор",
            500m);

        room.Services.Add(projector);

        conferenceRoomRepository.Rooms.Add(room);

        // Act
        var result = await conferenceRoomService.GetByIdAsync(room.Id);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(room.Id, result.Id);
        Assert.Equal("Зал A", result.Name);
        Assert.Equal(50, result.Capacity);
        Assert.Equal(2000m, result.BasePricePerHour);

        Assert.Single(result.Services);

        var service = result.Services[0];

        Assert.Equal(projector.Id, service.Id);
        Assert.Equal("Проектор", service.Name);
        Assert.Equal(500m, service.Price);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRoomDoesNotExist_ReturnsNull()
    {
        // Arrange
        var conferenceRoomRepoitory = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepoitory, bookingRepository);

        var roomId = Guid.NewGuid();

        //Act
        var result = await conferenceRoomService.GetByIdAsync(roomId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenRequestIsValid_UpdatesRoomAndServices()
    {
        // Arrage
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var projector = new Service(
            "Проектор",
            500m);

        var wiFi = new Service(
            "Wi-Fi",
            300m);

        var room = new ConferenceRoom(
            "Старый зал",
            30,
            1500m);

        room.Services.Add(projector);

        conferenceRoomRepository.Rooms.Add(room);

        conferenceRoomRepository.Services.Add(projector);
        conferenceRoomRepository.Services.Add(wiFi);

        var request = new UpdateConferenceRoomRequest
        {
            Name = "Большой зал",
            Capacity = 100,
            BasePricePerHour = 3500m,
            ServiceIds = new List<Guid>
            {
                wiFi.Id
            }
        };

        // Act
        await conferenceRoomService.UpdateAsync(room.Id, request);

        // Assert
        Assert.Equal("Большой зал", room.Name);
        Assert.Equal(100, room.Capacity);
        Assert.Equal(3500m, room.BasePricePerHour);

        Assert.Single(room.Services);
        Assert.Contains(wiFi, room.Services);
        Assert.DoesNotContain(projector, room.Services);
    }

    [Fact]
    public async Task UpdateAsync_WhenRoomDoesNotExist_ThrowsConferenceRoomNotFoundException()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var roomId = Guid.NewGuid();

        var request = new UpdateConferenceRoomRequest
        {
            Name = "Большой зал",
            Capacity = 100,
            BasePricePerHour = 3500m,
            ServiceIds = new List<Guid>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConferenceRoomNotFoundException>(
            () => conferenceRoomService.UpdateAsync(roomId, request));
    }

    [Fact]
    public async Task UpdateAsync_WhenServiceDoesNotExist_ThrowsServiceNotFoundException()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var room = new ConferenceRoom(
            "Зал A",
            50,
            2000m);

        conferenceRoomRepository.Rooms.Add(room);

        var projector = new Service(
            "Проектор",
            500m);

        conferenceRoomRepository.Services.Add(projector);

        var missingServiceId = Guid.NewGuid();

        var request = new UpdateConferenceRoomRequest
        {
            Name = "Большой зал",
            Capacity = 100,
            BasePricePerHour = 3500m,
            ServiceIds = new List<Guid>
        {
            missingServiceId
        }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ServiceNotFoundException>(
            () => conferenceRoomService.UpdateAsync(room.Id, request));

        // Проверяем, что комната не была изменена
        Assert.Equal("Зал A", room.Name);
        Assert.Equal(50, room.Capacity);
        Assert.Equal(2000m, room.BasePricePerHour);
        Assert.Empty(room.Services);
    }

    [Fact]
    public async Task DeleteAsync_WhenRoomExists_DeletesRoom()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var room = new ConferenceRoom(
            "Зал A",
            50,
            2000m);

        conferenceRoomRepository.Rooms.Add(room);

        // Act
        await conferenceRoomService.DeleteAsync(room.Id);

        // Assert
        Assert.Empty(conferenceRoomRepository.Rooms);
    }

    [Fact]
    public async Task DeleteAsync_WhenRoomDoesNotExist_ThrowsConferenceRoomNotFoundException()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var roomId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<ConferenceRoomNotFoundException>(
            () => conferenceRoomService.DeleteAsync(roomId));
    }

    [Fact]
    public async Task SearchAvailableAsync_WhenRoomIsFree_ReturnsOnlyAvailableRooms()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var roomA = new ConferenceRoom(
            "Зал A",
            50,
            2000m);

        var roomB = new ConferenceRoom(
            "Зал B",
            100,
            3500m);

        conferenceRoomRepository.Rooms.Add(roomA);
        conferenceRoomRepository.Rooms.Add(roomB);

        var existingBooking = new Booking(
            roomA.Id,
            new DateTime(2026, 9, 20, 10, 0, 0),
            new DateTime(2026, 9, 20, 14, 0, 0),
            2000m,
            4000m);

        bookingRepository.Bookings.Add(existingBooking);

        var request = new SearchAvailableRoomsRequest
        {
            StartTime = new DateTime(2026, 9, 20, 11, 0, 0),
            EndTime = new DateTime(2026, 9, 20, 13, 0, 0),
            Capacity = 50
        };

        // Act
        var result = await conferenceRoomService.SearchAvailableAsync(request);

        // Assert
        Assert.Single(result);
        Assert.Equal(roomB.Id, result[0].Id);
    }

    [Fact]
    public async Task SearchAvailableAsync_WhenRoomCapacityIsTooSmall_ExcludesRoom()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var roomA = new ConferenceRoom(
            "Зал A",
            50,
            2000m);

        var roomB = new ConferenceRoom(
            "Зал B",
            100,
            3500m);

        conferenceRoomRepository.Rooms.Add(roomA);
        conferenceRoomRepository.Rooms.Add(roomB);

        var request = new SearchAvailableRoomsRequest
        {
            StartTime = new DateTime(2026, 9, 20, 10, 0, 0),
            EndTime = new DateTime(2026, 9, 20, 14, 0, 0),
            Capacity = 80
        };

        // Act
        var result = await conferenceRoomService.SearchAvailableAsync(request);

        // Assert
        Assert.Single(result);
        Assert.Equal(roomB.Id, result[0].Id);
    }

    [Fact]
    public async Task SearchAvailableAsync_WhenRoomCapacityEqualsRequestedCapacity_ReturnsRoom()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var room = new ConferenceRoom(
            "Зал A",
            50,
            2000m);

        var room2 = new ConferenceRoom(
            "Зал B",
            35,
            2000m);

        conferenceRoomRepository.Rooms.Add(room);
        conferenceRoomRepository.Rooms.Add(room2);

        var request = new SearchAvailableRoomsRequest
        {
            StartTime = new DateTime(2026, 9, 20, 10, 0, 0),
            EndTime = new DateTime(2026, 9, 20, 14, 0, 0),
            Capacity = 50
        };

        // Act
        var result = await conferenceRoomService.SearchAvailableAsync(request);

        // Assert
        Assert.Single(result);
        Assert.Equal(room.Id, result[0].Id);
    }

    [Fact]
    public async Task SearchAvailableAsync_WhenStartTimeIsAfterEndTime_ThrowsInvalidBookingPeriodException()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var request = new SearchAvailableRoomsRequest
        {
            StartTime = new DateTime(2026, 9, 20, 14, 0, 0),
            EndTime = new DateTime(2026, 9, 20, 10, 0, 0),
            Capacity = 50
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidBookingPeriodException>(
            () => conferenceRoomService.SearchAvailableAsync(request));
    }

    [Fact]
    public async Task SearchAvailableAsync_WhenStartTimeEqualsEndTime_ThrowsInvalidBookingPeriodException()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var request = new SearchAvailableRoomsRequest
        {
            StartTime = new DateTime(2026, 9, 20, 14, 0, 0),
            EndTime = new DateTime(2026, 9, 20, 14, 0, 0),
            Capacity = 50
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidBookingPeriodException>(
            () => conferenceRoomService.SearchAvailableAsync(request));
    }

    [Fact]
    public async Task SearchAvailableAsync_WhenCapacityIsZero_ThrowsInvalidCapacityException()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var request = new SearchAvailableRoomsRequest
        {
            StartTime = new DateTime(2026, 9, 20, 10, 0, 0),
            EndTime = new DateTime(2026, 9, 20, 14, 0, 0),
            Capacity = 0
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCapacityException>(
            () => conferenceRoomService.SearchAvailableAsync(request));
    }

    [Fact]
    public async Task SearchAvailableAsync_WhenCapacityIsNegative_ThrowsInvalidCapacityException()
    {
        // Arrange
        var conferenceRoomRepository = new FakeConferenceRoomRepository();
        var bookingRepository = new FakeBookingRepository();

        var conferenceRoomService = new ConferenceRoomService(
            conferenceRoomRepository,
            bookingRepository);

        var request = new SearchAvailableRoomsRequest
        {
            StartTime = new DateTime(2026, 9, 20, 10, 0, 0),
            EndTime = new DateTime(2026, 9, 20, 14, 0, 0),
            Capacity = -1
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCapacityException>(
            () => conferenceRoomService.SearchAvailableAsync(request));
    }
}