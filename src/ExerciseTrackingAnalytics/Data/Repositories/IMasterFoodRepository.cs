using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Data.Repositories
{
    public interface IMasterFoodRepository
    {
        Task<MasterFood?> GetByIdAsync(long masterFoodId);

        Task<MasterFood> InsertAsync(MasterFood masterFood);

        Task<IEnumerable<MasterFood>> SearchAsync(Guid contextUserId, string? searchTerm, string? barcode);
    }
}
