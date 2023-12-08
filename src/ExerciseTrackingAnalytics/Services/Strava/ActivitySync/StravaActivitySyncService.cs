using ExerciseTrackingAnalytics.Data.Repositories;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models;
using ExerciseTrackingAnalytics.Models.Strava;
using ExerciseTrackingAnalytics.Services.Strava.API;
using static ExerciseTrackingAnalytics.Constants;

namespace ExerciseTrackingAnalytics.Services.Strava.ActivitySync
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
            _logger.LogInformation("Fetching recent Strava Activities ...");

            var syncOperationResult = new StravaSyncStatus() { IsSuccessful = false, NumSyncedActivities = 0 };
            StravaActivity[] recentStravaActivities;

            try
            {
                recentStravaActivities = (await _stravaApi.GetRecentActivitiesAsync()).ToArray();
                _logger.LogInformation("Found {count} recent activities", recentStravaActivities.Length);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recent Strava Activities");
                return syncOperationResult;
            }

            foreach (var activity in recentStravaActivities)
            {
                activity.IsSynced = await _repository.ExistsByExternalAppIdAsync(ExerciseTrackingApp.Strava, activity.Id);
            }

            if (recentStravaActivities.Any(a => !a.IsSynced))
            {
                var numSyncedSuccessfully = 0;

                var unsyncedActivities = recentStravaActivities
                    .Where(a => !a.IsSynced)
                    .OrderBy(a => a.StartDateUtc)
                    .ToArray();

                _logger.LogInformation("{count} new Activities have not yet been synced to the database.", unsyncedActivities.Length);

                try
                {
                    foreach (var unsyncedActivity in unsyncedActivities)
                    {
                        _logger.LogInformation("Fetching details to hydrate Calories for Activity ID {stravaActivityId} ... ", unsyncedActivity.Id);

                        await _stravaApi.HydrateActivityCaloriesAsync(unsyncedActivity);

                        _logger.LogInformation("Writing Activity ID {stravaActivityId} to the database ... ", unsyncedActivity.Id);

                        var userActivity = new UserActivity()
                        {
                            UserId = _contextAccessor.HttpContext!.User.GetUserId(),
                            ExternalApp = ExerciseTrackingApp.Strava,
                            ExternalAppActivityId = unsyncedActivity.Id,
                            Name = unsyncedActivity.Name ?? "Not Specified",
                            SportType = unsyncedActivity.SportType ?? "Unknown",
                            StartDateUtc = DateTime.SpecifyKind(unsyncedActivity.StartDateUtc, DateTimeKind.Unspecified), // EF + Npgsql are insisting that `Kind` has to be `Unspecified` to write to `TIMESTAMP` column (it wants type `TIMESTAMPTZ` if `Kind` is `Utc`)
                            TimeZone = !string.IsNullOrWhiteSpace(unsyncedActivity.Timezone)
                                ? unsyncedActivity.Timezone.Split(new char[] { ' ' })[1] // Strava sends values like [Offset] [TimeZoneId], e.g. "(GMT-08:00) America/Los_Angeles"
                                : "America/Los_Angeles",
                            Calories = unsyncedActivity.Calories,
                            DistanceInMeters = unsyncedActivity.Distance,
                            DistanceInMiles = unsyncedActivity.Distance * MilesPerMeter,
                            DistanceOriginalUnit = DistanceUnit.Meters,
                            ElapsedTimeInSeconds = unsyncedActivity.ElapsedTimeInSeconds,
                            MovingTimeInSeconds = unsyncedActivity.MovingTimeInSeconds,
                            TotalElevationGainInMeters = unsyncedActivity.TotalElevationGainInMeters,
                        };

                        var savedActivity = await _repository.InsertAsync(userActivity);

                        if (savedActivity != null)
                            ++numSyncedSuccessfully;
                    }

                    syncOperationResult.IsSuccessful = numSyncedSuccessfully == unsyncedActivities.Length;
                    syncOperationResult.NumSyncedActivities = numSyncedSuccessfully;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error syncing recent Strava Activities");
                    return syncOperationResult;
                }
            }
            else
            {
                _logger.LogInformation("All recent activities have been synced to the database");
                syncOperationResult.IsSuccessful = true;
            }

            return syncOperationResult;
        }
    }
}
