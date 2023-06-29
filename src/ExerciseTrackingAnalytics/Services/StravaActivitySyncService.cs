using ExerciseTrackingAnalytics.Data.Repositories;
using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Services
{
    public class StravaActivitySyncService : IStravaActivitySyncService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<StravaActivitySyncService> _logger;
        private readonly IStravaApiService _stravaApi;
        private readonly IUserActivityRepository _repository;

        public StravaActivitySyncService(
            IHttpContextAccessor contextAccessor,
            ILogger<StravaActivitySyncService> logger,
            IStravaApiService stravaApi,
            IUserActivityRepository repository)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _stravaApi = stravaApi;
            _repository = repository;
        }

        public async Task<StravaSyncStatus> SyncRecentActivitiesAsync()
        {
            _logger.LogInformation("Fethcing recent Strava Activities...");

            var syncOperationResult = new StravaSyncStatus() {  IsSuccessful = false, NumSyncedActivities = 0 };
            IEnumerable<StravaActivity> recentStravaActivities;

            try
            {
                recentStravaActivities = await _stravaApi.GetRecentActivitiesAsync();
                _logger.LogInformation("Found {count} recent activities", recentStravaActivities.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recent Strava Activities");
                return syncOperationResult;
            }



            return syncOperationResult;
        }
    }
}
