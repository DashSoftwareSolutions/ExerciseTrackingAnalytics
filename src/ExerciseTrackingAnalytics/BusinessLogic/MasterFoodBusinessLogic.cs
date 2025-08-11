using ExerciseTrackingAnalytics.Data.Repositories;
using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.BusinessLogic
{
    public class MasterFoodBusinessLogic : IMasterFoodBusinessLogic
    {
        private readonly ILogger<MasterFoodBusinessLogic> _logger;
        private readonly IMasterFoodRepository _masterFoodRepository;

        public MasterFoodBusinessLogic(ILogger<MasterFoodBusinessLogic> logger, IMasterFoodRepository masterFoodRepository)
        {
            _logger = logger;
            _masterFoodRepository = masterFoodRepository;
        }

        public async Task<BusinessLogicResponse<IEnumerable<MasterFood>>> Search(Guid contextUserId, string? searchTerm, string? barcode)
        {
            try
            {
                var results = await _masterFoodRepository.SearchAsync(contextUserId, searchTerm, barcode);
                return new(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search for foods");
                return new(ErrorType.RuntimeException, "We're sorry.  We were unable to fetch the list of foods due to an internal error.  Please try your request again.  If the problem persists, please contact support.");
            }
        }
    }
}
