namespace ConferenceRoomBooking.Application.Services;

public class BookingPriceCalculator
{
    public decimal Calculate(DateTime startTime, DateTime endTime, decimal basePricePerHour)
    {
        decimal total = 0;

        var currentTime = startTime;

        while (currentTime < endTime)
        {
            var nextBoundary = GetNextTariffBoundary(currentTime);

            if (nextBoundary > endTime)
            {
                nextBoundary = endTime;
            }

            var hours = (decimal)(nextBoundary - currentTime).TotalHours;

            var multiplier = GetPriceMultiplier(currentTime);

            total += basePricePerHour * hours * multiplier;

            currentTime = nextBoundary;
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

    private DateTime GetNextTariffBoundary(DateTime time)
    {
        var date = time.Date;
        var currentTime = time.TimeOfDay;

        var boundaries = new[]
        {
            TimeSpan.FromHours(9),
            TimeSpan.FromHours(12),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(18),
            TimeSpan.FromHours(23)
        };

        foreach (var boundary in boundaries)
        {
            if (boundary > currentTime)
            {
                return date.Add(boundary);
            }
        }

        return date.AddDays(1).AddHours(6);
    }
}