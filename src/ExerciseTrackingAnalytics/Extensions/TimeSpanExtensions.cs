namespace ExerciseTrackingAnalytics.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToHoursMinutesSecondsString(this TimeSpan timeSpan)
        {
            var totalHours = (int)Math.Floor(timeSpan.TotalHours);

            return $"{totalHours.ToString("D2")}:{timeSpan.Minutes.ToString("D2")}:{timeSpan.Seconds.ToString("D2")}";
        }
    }
}
