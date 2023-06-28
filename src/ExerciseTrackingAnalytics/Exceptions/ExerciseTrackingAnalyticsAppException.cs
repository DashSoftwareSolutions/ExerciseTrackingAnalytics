namespace ExerciseTrackingAnalytics.Exceptions
{
    /// <summary>
    /// Custom exception type for the application
    /// </summary>
    public class ExerciseTrackingAnalyticsAppException : Exception
    {
        public ExerciseTrackingAnalyticsAppException() { }

        public ExerciseTrackingAnalyticsAppException(string message) : base(message) { }
    }
}
