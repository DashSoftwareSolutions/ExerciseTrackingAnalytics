namespace ExerciseTrackingAnalytics.Models.Strava
{
    /// <summary>
    /// Represents the Strava Athlete (User) association on an owned resource such as Activity
    /// </summary>
    /// <remarks>
    /// <see href="https://developers.strava.com/docs/reference/#api-models-MetaAthlete"/>
    /// <see href="https://developers.strava.com/docs/reference/#api-models-SummaryActivity"/><br />
    /// <see href="https://developers.strava.com/docs/reference/#api-models-DetailedActivity"/>
    /// </remarks>
    public class StravaMetaAthlete
    {
        public long Id { get; set; }
    }
}
