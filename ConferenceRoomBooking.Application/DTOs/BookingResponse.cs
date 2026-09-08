namespace ConferenceRoomBooking.Application.DTOs;

public class BookingResponse
{
    public Guid Id { get; set; }
    public Guid ConferenceRoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal RoomPrice { get; set; }
    public decimal ServicesPrice { get; set; }
    public decimal TotalPrice { get; set; }
}