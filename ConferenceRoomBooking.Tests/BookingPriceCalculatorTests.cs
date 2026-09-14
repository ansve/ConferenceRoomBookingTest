using ConferenceRoomBooking.Application.Services;

namespace ConferenceRoomBooking.Tests;

public class BookingPriceCalculatorTests
{
    [Fact]
    public void Calculate_TenToTwelve_ReturnsFourThousand()
    {
        // Arrange
        var calculator = new BookingPriceCalculator();

        var startTime = new DateTime(2026, 9, 9, 10, 0, 0);
        var endTime = new DateTime(2026, 9, 9, 12, 0, 0);
        var basePricePerHour = 2000m;

        // Act
        var result = calculator.Calculate(
            startTime,
            endTime,
            basePricePerHour);

        // Assert
        Assert.Equal(4000m, result);
    }

    [Fact]
    public void Calculate_TwelveToFourteen_ReturnsFourThousandSixHundred()
    {
        // Arrange
        var calculator = new BookingPriceCalculator();

        var startTime = new DateTime(2026, 9, 9, 12, 0, 0);
        var endTime = new DateTime(2026, 9, 9, 14, 0, 0);
        var basePricePerHour = 2000m;

        // Act
        var result = calculator.Calculate(
            startTime,
            endTime,
            basePricePerHour);

        // Assert
        Assert.Equal(4600m, result);
    }

    [Fact]
    public void Calculate_ElevenThirtyToFourteenThirty_ReturnsSixThousandSixHundred()
    {
        // Arrange
        var calculator = new BookingPriceCalculator();

        var startTime = new DateTime(2026, 9, 9, 11, 30, 0);
        var endTime = new DateTime(2026, 9, 9, 14, 30, 0);
        var basePricePerHour = 2000m;

        // Act
        var result = calculator.Calculate(
            startTime,
            endTime,
            basePricePerHour);

        // Assert
        Assert.Equal(6600m, result);
    }

    [Fact]
    public void Calculate_SevenToNine_ReturnsThreeThousandSixHundred()
    {
        // Arrange
        var calculator = new BookingPriceCalculator();

        var startTime = new DateTime(2026, 9, 9, 7, 0, 0);
        var endTime = new DateTime(2026, 9, 9, 9, 0, 0);
        var basePricePerHour = 2000m;

        // Act
        var result = calculator.Calculate(
            startTime,
            endTime,
            basePricePerHour);

        // Assert
        Assert.Equal(3600m, result);
    }

    [Fact]
    public void Calculate_EighteenToTwenty_ReturnsThreeThousandTwoHundred()
    {
        // Arrange
        var calculator = new BookingPriceCalculator();

        var startTime = new DateTime(2026, 9, 9, 18, 0, 0);
        var endTime = new DateTime(2026, 9, 9, 20, 0, 0);
        var basePricePerHour = 2000m;

        // Act
        var result = calculator.Calculate(
            startTime,
            endTime,
            basePricePerHour);

        // Assert
        Assert.Equal(3200m, result);
    }
}