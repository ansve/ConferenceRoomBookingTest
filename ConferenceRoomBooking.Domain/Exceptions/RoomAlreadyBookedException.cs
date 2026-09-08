namespace ConferenceRoomBooking.Domain.Exceptions;

public class RoomAlreadyBookedException : BusinessException
{
    public RoomAlreadyBookedException()
        : base("Conference room is already booked for the selected period.")
    {
    }
}