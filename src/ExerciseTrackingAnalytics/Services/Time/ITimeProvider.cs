namespace ExerciseTrackingAnalytics.Services.Time
{
    /// <summary>
    /// Provides an abstraction in front of getting the current time from the system.<br />
    /// That allows a controllable dummy Time Provider to be used when unit testing,
    /// so as to be able to simulate specific times that may differ from actual system time.
    /// </summary>
    public interface ITimeProvider
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}
