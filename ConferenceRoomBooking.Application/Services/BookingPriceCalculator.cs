namespace ConferenceRoomBooking.Application.Services;

public class BookingPriceCalculator
{
    public decimal Calculate(
        DateTime startTime,
        DateTime endTime,
        decimal basePricePerHour)
    {
        decimal total = 0;

        var currentTime = startTime;

        while (currentTime < endTime)
        {
            var nextHour = currentTime.AddHours(1);

            if (nextHour > endTime)
            {
                nextHour = endTime;
            }

            var hours = (decimal)(nextHour - currentTime).TotalHours;

            var multiplier = GetPriceMultiplier(currentTime);

            total += basePricePerHour * hours * multiplier;

            currentTime = nextHour;
        }

        return total;
    }

    private decimal GetPriceMultiplier(DateTime time)
    {
        var currentTime = time.TimeOfDay;

        if (currentTime >= TimeSpan.FromHours(12) &&
            currentTime < TimeSpan.FromHours(14))
        {
            return 1.15m;
        }

        if (currentTime >= TimeSpan.FromHours(6) &&
            currentTime < TimeSpan.FromHours(9))
        {
            return 0.90m;
        }

        if (currentTime >= TimeSpan.FromHours(18) &&
            currentTime < TimeSpan.FromHours(23))
        {
            return 0.80m;
        }

        return 1.00m;
    }
}