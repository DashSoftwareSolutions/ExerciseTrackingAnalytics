namespace ExerciseTrackingAnalytics.Services
{
    /// <summary>
    /// Service to sync recent Activities from Strava to the application database for the context user
    /// </summary>
    public interface IStravaActivitySyncService
    {
        /// <summary>
        /// Syncs recent Activities from Strava to the application database for the context user
        /// </summary>
        Task<StravaSyncStatus> SyncRecentActivitiesAsync();
    }
}
