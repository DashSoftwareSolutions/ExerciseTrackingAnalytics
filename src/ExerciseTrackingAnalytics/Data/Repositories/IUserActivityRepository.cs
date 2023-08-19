using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Data.Repositories
{
    public interface IUserActivityRepository
    {
        Task<UserActivity?> GetByIdAsync(long id);

        Task<UserActivity?> GetByExternalAppIdAsync(ExerciseTrackingApp externalApp, long externalAppActivityId);

        Task<bool> ExistsByExternalAppIdAsync(ExerciseTrackingApp externalApp, long externalAppActivityId);

        Task<UserActivity?> InsertAsync(UserActivity userActivity);
    }
}
