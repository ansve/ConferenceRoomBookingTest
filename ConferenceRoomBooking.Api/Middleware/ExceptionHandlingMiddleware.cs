using ConferenceRoomBooking.Domain.Exceptions;

namespace ConferenceRoomBooking.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException exception)
        {
            context.Response.StatusCode = exception switch
            {
                InvalidBookingPeriodException => StatusCodes.Status400BadRequest,
                RoomAlreadyBookedException => StatusCodes.Status409Conflict,
                ConferenceRoomNotFoundException => StatusCodes.Status404NotFound,
                ServiceNotFoundException => StatusCodes.Status404NotFound,
                InvalidCapacityException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status400BadRequest
            };

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                error = exception.Message
            });
        }
    }
}