using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Data.Repositories
{
    public interface IMasterFoodRepository
    {
        Task<MasterFood?> GetByIdAsync(long masterFoodId);

        Task<MasterFood> InsertAsync(MasterFood masterFood);
    }
}
