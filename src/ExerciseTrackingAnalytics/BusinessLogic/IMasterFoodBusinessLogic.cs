using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.BusinessLogic
{
    public interface IMasterFoodBusinessLogic
    {
        Task<BusinessLogicResponse<IEnumerable<MasterFood>>> Search(Guid contextUserId, string? searchTerm, string? barcode);
    }
}
