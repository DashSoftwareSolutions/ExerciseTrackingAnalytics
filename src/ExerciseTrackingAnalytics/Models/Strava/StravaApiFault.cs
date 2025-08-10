namespace ExerciseTrackingAnalytics.Models.Strava
{
    /// <summary>
    /// Represents a problem encountered when calling the Strava API
    /// </summary>
    public class StravaApiFault
    {
        public IEnumerable<StravaApiError>? Errors { get; set; }
        public string? Message { get; set; }
    }
}
