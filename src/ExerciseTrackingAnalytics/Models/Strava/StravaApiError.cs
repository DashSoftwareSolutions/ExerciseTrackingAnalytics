namespace ExerciseTrackingAnalytics.Models.Strava
{
    /// <summary>
    /// Represents specific error details about a problem ancountered when calling the Strava API
    /// </summary>
    /// <remarks>
    /// <see href="https://developers.strava.com/docs/reference/#api-models-Error"/>
    /// </remarks>
    public class StravaApiError
    {
        public string? Code { get; set; }
        public string? Field { get; set; }
        public string? Resource { get; set; }
    }
}
