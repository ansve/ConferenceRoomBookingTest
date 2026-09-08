namespace ConferenceRoomBooking.Application.DTOs;

public class ConferenceRoomResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BasePricePerHour { get; set; }
    public List<ServiceResponse> Services { get; set; } = new();
}