namespace ConferenceRoomBooking.Domain.Entities;

public class BookingService
{
    public Guid Id { get; private set; }
    public Guid BookingId { get; private set; }
    public Guid ServiceId { get; private set; }
    public decimal Price { get; private set; }

    public BookingService(Guid bookingId, Guid serviceId, decimal price)
    {
        Id = Guid.NewGuid();
        BookingId = bookingId;
        ServiceId = serviceId;
        Price = price;
    }
}