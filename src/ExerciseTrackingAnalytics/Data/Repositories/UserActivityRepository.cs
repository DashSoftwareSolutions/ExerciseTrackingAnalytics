using Microsoft.EntityFrameworkCore;
using Dapper;
using Npgsql;
using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Data.Repositories
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UserActivityRepository> _logger;

        public UserActivityRepository(ApplicationDbContext db, ILogger<UserActivityRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public Task<ActivityAggregateStatistics> GetActivityAggregateStatisticsAsync(
            Guid userId,
            DateTime? dateRangeStartUtc,
            DateTime? dateRangeEndUtc)
        {
            using (var connection = new NpgsqlConnection(_db.Database.GetConnectionString()))
            {
                return connection.QuerySingleAsync<ActivityAggregateStatistics>(@"
  SELECT COUNT(*) AS ""NumActivities""
        ,COALESCE(SUM(""DistanceInMeters""), 0) AS ""TotalDistanceInMeters""
        ,COALESCE(SUM(""DistanceInMiles""), 0) AS ""TotalDistanceInMiles""
        ,COALESCE(SUM(""TotalElevationGainInMeters""), 0) AS ""TotalElevationGainInMeters""
        ,COALESCE(SUM(""ElapsedTimeInSeconds""), 0) AS ""TotalElaspedTimeInSeconds""
        ,COALESCE(SUM(""MovingTimeInSeconds""), 0) AS ""TotalMovingTimeInSeconds""
        ,COALESCE(SUM(""Calories""), 0) AS ""TotalCalories""
    FROM ""UserActivities""
   WHERE ""UserId"" = @userId
     AND (@dateRangeStartUtc IS NULL OR ""StartDateUtc"" >= @dateRangeStartUtc)
     AND (@dateRangeEndUtc IS NULL OR ""StartDateUtc"" <= @dateRangeEndUtc);
",
                    new
                    {
                        userId,
                        dateRangeStartUtc,
                        dateRangeEndUtc,
                    });
            }
        }

        public Task<bool> ExistsByExternalAppIdAsync(ExerciseTrackingApp externalApp, long externalAppActivityId)
        {
            return _db.UserActivities!.AnyAsync(a =>
                a.ExternalApp == externalApp &&
                a.ExternalAppActivityId == externalAppActivityId);
        }

        public Task<UserActivity?> GetByIdAsync(long id)
        {
            return _db.UserActivities!.FirstOrDefaultAsync(a => a.Id == id);
        }

        public Task<UserActivity?> GetByExternalAppIdAsync(ExerciseTrackingApp externalApp, long externalAppActivityId)
        {
            return _db.UserActivities!.FirstOrDefaultAsync(a =>
                a.ExternalApp == externalApp &&
                a.ExternalAppActivityId == externalAppActivityId);
        }

        public async Task<UserActivity?> InsertAsync(UserActivity userActivity)
        {
            await _db.UserActivities!.AddAsync(userActivity);
            await _db.SaveChangesAsync();
            return await GetByIdAsync(userActivity.Id);
        }
    }
}
