namespace ConferenceRoomBooking.Domain.Exceptions;

public class InvalidCapacityException : BusinessException
{
    public InvalidCapacityException(int capacity)
        : base($"Capacity must be greater than zero. Actual value: '{capacity}'.")
    {
    }
}