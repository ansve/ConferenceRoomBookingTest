

namespace ConferenceRoomBooking.Application.DTOs;

public class CreateConferenceRoomRequest
{
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BasePricePerHour { get; set; }
    public List<Guid> ServiceIds { get; set; } = new();
}
