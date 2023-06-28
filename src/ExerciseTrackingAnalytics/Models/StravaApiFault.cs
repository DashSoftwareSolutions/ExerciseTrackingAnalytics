namespace ExerciseTrackingAnalytics.Models
{
    /// <summary>
    /// Represents a problem ancountered when calling the Strava API
    /// </summary>
    public class StravaApiFault
    {
        public IEnumerable<StravaApiError>? Errors { get; set; }
        public string? Message { get; set; }
    }
}
