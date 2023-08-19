using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExerciseTrackingAnalytics.Models.Strava
{
    /// <summary>
    /// Represents an Activity in Strava
    /// </summary>
    /// <remarks>
    /// Mostly represents a <b><c>SummaryActivity</c></b>, but also includes the Calories, which is only available on <b><c>DetailedActivity</c></b>.<br />
    /// <br />
    /// See:<br />
    /// <see href="https://developers.strava.com/docs/reference/#api-models-SummaryActivity"/><br />
    /// <see href="https://developers.strava.com/docs/reference/#api-models-DetailedActivity"/>
    /// </remarks>
    public class StravaActivity
    {
        public bool IsSynced { get; set; }

        public long Id { get; set; }

        [JsonProperty("athlete")]
        public StravaMetaAthlete? MetaAthlete { get; set; }

        public string? Name { get; set; }

        [JsonProperty("sport_type")]
        public string? SportType { get; set; }

        [JsonProperty("start_date")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime StartDateUtc { get; set; }

        public string? Timezone { get; set; }

        /// <summary>
        /// Distance is in Meters
        /// </summary>
        public decimal Distance { get; set; }

        [JsonProperty("total_elevation_gain")]
        public decimal TotalElevationGainInMeters { get; set; }

        [JsonProperty("elapsed_time")]
        public int ElapsedTimeInSeconds { get; set; }

        public TimeSpan ElapsedTime => TimeSpan.FromSeconds(ElapsedTimeInSeconds);

        [JsonProperty("moving_time")]
        public int MovingTimeInSeconds { get; set; }

        public TimeSpan MovingTime => TimeSpan.FromSeconds(MovingTimeInSeconds);

        public decimal Calories { get; set; }
    }
}
