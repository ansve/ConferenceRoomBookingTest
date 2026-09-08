namespace ConferenceRoomBooking.Domain.Exceptions;

public class InvalidBookingPeriodException : BusinessException
{
    public InvalidBookingPeriodException(
        DateTime startTime,
        DateTime endTime)
        : base(
            $"Invalid booking period: start time '{startTime}' must be earlier than end time '{endTime}'.")
    {
    }
}