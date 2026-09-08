namespace ConferenceRoomBooking.Domain.Exceptions;

public class ServiceNotFoundException : BusinessException
{
    public ServiceNotFoundException(Guid serviceId)
        : base($"Service with id '{serviceId}' was not found.")
    {
    }
}