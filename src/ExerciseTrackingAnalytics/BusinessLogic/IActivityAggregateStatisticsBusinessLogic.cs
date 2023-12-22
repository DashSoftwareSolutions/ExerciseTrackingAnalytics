using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.BusinessLogic
{
    public interface IActivityAggregateStatisticsBusinessLogic
    {
        Task<BusinessLogicResponse<ActivityDashboardData>> GetActivityAggregateStatisticsAsync(Guid userId, string userTimeZone);
    }
}
