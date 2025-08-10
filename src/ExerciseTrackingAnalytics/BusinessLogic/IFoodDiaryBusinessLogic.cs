using ExerciseTrackingAnalytics.Models;
using static ExerciseTrackingAnalytics.Constants;

namespace ExerciseTrackingAnalytics.BusinessLogic
{
    public interface IFoodDiaryBusinessLogic
    {
        Task<BusinessLogicResponse<FoodDiaryEntry>> CreateFoodDiaryEntry(FoodDiaryEntry foodDiaryEntry);

        Task<BusinessLogicResponse<FoodDiaryDailySummary>> GetDailySummary(Guid userId, DateOnly date, string userTimeZone = DefaultTimeZoneId);
    }
}
