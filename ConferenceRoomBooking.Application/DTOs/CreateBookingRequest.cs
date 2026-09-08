namespace ConferenceRoomBooking.Application.DTOs;

public class CreateBookingRequest
{
    public Guid ConferenceRoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<Guid> ServiceIds { get; set; } = new();
}