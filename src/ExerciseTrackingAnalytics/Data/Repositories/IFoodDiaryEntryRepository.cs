using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Data.Repositories
{
    public interface IFoodDiaryEntryRepository
    {
        Task<FoodDiaryEntry?> GetByIdAsync(long foodDiaryEntryId);

        Task<IEnumerable<FoodDiaryEntry>> GetByUserAndDateAsync(Guid userId, DateOnly date);

        Task<IEnumerable<FoodDiaryEntry>> GetByUserAndDateRangeAsync(Guid userId, DateOnly startDate, DateOnly endDate);

        Task<FoodDiaryEntry> InsertAsync(FoodDiaryEntry foodDiaryEntry);
    }
}
