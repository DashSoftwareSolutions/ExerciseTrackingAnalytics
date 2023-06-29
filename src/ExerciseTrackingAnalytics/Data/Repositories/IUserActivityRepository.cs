using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Data.Repositories
{
    public interface IUserActivityRepository
    {
        Task<UserActivity?> GetByIdAsync(long id);

        Task<UserActivity?> GetByStravaId(long stravaActivityId);

        Task<bool> ExistsByStravaIdAsync(long stravaActivityId);

        Task<UserActivity?> InsertAsync(UserActivity userActivity);
    }
}
