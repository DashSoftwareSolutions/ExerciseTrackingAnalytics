using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Data.Repositories
{
    public interface IUserActivityRepository
    {
        Task<ActivityAggregateStatistics> GetActivityAggregateStatisticsAsync(
            Guid userId,
            DateTime? dateRangeStartUtc,
            DateTime? dateRangeEndUtc);

        Task<UserActivity?> GetByIdAsync(long id);

        Task<UserActivity?> GetByExternalAppIdAsync(ExerciseTrackingApp externalApp, long externalAppActivityId);

        Task<IEnumerable<UserActivity>> GetByUserAndDateRange(Guid userId, DateTime dateRangeStartUtc, DateTime dateRangeEndUtc);

        Task<bool> ExistsByExternalAppIdAsync(ExerciseTrackingApp externalApp, long externalAppActivityId);

        Task<UserActivity?> InsertAsync(UserActivity userActivity);
    }
}
