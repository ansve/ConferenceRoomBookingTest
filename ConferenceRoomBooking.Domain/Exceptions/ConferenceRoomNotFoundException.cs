namespace ConferenceRoomBooking.Domain.Exceptions;

public class ConferenceRoomNotFoundException : BusinessException
{
    public ConferenceRoomNotFoundException(Guid roomId)
        : base($"Conference room with id '{roomId}' was not found.")
    {
    }
}