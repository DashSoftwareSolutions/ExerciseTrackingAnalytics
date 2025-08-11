using System.Collections.Immutable;
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

        public async Task<ActivityAggregateStatistics> GetActivityAggregateStatisticsAsync(
            Guid userId,
            DateTime? dateRangeStartUtc,
            DateTime? dateRangeEndUtc)
        {
            using (var connection = new NpgsqlConnection(_db.Database.GetConnectionString()))
            {
                return await connection.QuerySingleAsync<ActivityAggregateStatistics>(@"
  SELECT COUNT(*) AS ""NumActivities""
        ,COALESCE(SUM(""DistanceInMeters""), 0) AS ""TotalDistanceInMeters""
        ,COALESCE(SUM(""DistanceInMiles""), 0) AS ""TotalDistanceInMiles""
        ,COALESCE(SUM(""TotalElevationGainInMeters""), 0) AS ""TotalElevationGainInMeters""
        ,COALESCE(SUM(""ElapsedTimeInSeconds""), 0) AS ""TotalElapsedTimeInSeconds""
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
                        //dateRangeStartUtc,
                        //dateRangeEndUtc,
                        // Sigh.  For some reason, Npgsql keeps trying to outsmart me when it comes to time zones. So, have to change Kind = Utc to Kind = Unspecified to ensure my values are taken verbatim without being messed with.  Gross. :-(
                        dateRangeStartUtc = dateRangeStartUtc.HasValue ? DateTime.SpecifyKind(dateRangeStartUtc.Value, DateTimeKind.Unspecified) : (DateTime?)null,
                        dateRangeEndUtc = dateRangeEndUtc.HasValue ? DateTime.SpecifyKind(dateRangeEndUtc.Value, DateTimeKind.Unspecified) : (DateTime?)null,
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

        public async Task<IEnumerable<UserActivity>> GetByUserAndDateRange(Guid userId, DateTime dateRangeStartUtc, DateTime dateRangeEndUtc)
        {
            var results = await _db
                .UserActivities!
                .Where(ua => ua.UserId == userId && ua.StartDateUtc >= dateRangeStartUtc && ua.StartDateUtc <= dateRangeEndUtc)
                .OrderBy(ua => ua.StartDateUtc)
                .ToArrayAsync();

            return results.ToImmutableArray().AsEnumerable();
        }

        public async Task<UserActivity?> InsertAsync(UserActivity userActivity)
        {
            await _db.UserActivities!.AddAsync(userActivity);
            await _db.SaveChangesAsync();
            return await GetByIdAsync(userActivity.Id);
        }
    }
}
