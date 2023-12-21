using ExerciseTrackingAnalytics.Extensions;

namespace ExerciseTrackingAnalytics.Tests.Extensions
{
    public class TimeSpanExtensionsFixture
    {
        [Theory]
        [InlineData("3:45:23", "03:45:23")]
        [InlineData("1.01:23:52", "25:23:52")]
        public void ToHoursMinutesSecondsString_Works(string parseableTimeSpan, string expectedOutput)
        {
            Assert.Equal(expectedOutput, TimeSpan.Parse(parseableTimeSpan).ToHoursMinutesSecondsString());
        }
    }
}
