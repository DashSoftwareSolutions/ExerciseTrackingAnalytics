using ExerciseTrackingAnalytics.Data.Repositories;
using ExerciseTrackingAnalytics.Models;
using static ExerciseTrackingAnalytics.Constants;

namespace ExerciseTrackingAnalytics.BusinessLogic
{
    public class FoodDiaryBusinessLogic : IFoodDiaryBusinessLogic
    {
        private readonly ILogger<FoodDiaryBusinessLogic> _logger;
        private readonly IFoodDiaryEntryRepository _foodDiaryEntryRepository;
        private readonly IMasterFoodRepository _masterFoodRepository;
        private readonly IUserActivityRepository _userActivityRepository;

        public FoodDiaryBusinessLogic(
            ILogger<FoodDiaryBusinessLogic> logger,
            IFoodDiaryEntryRepository foodDiaryEntryRepository,
            IMasterFoodRepository masterFoodRepository,
            IUserActivityRepository userActivityRepository)
        {
            _logger = logger;
            _foodDiaryEntryRepository = foodDiaryEntryRepository;
            _masterFoodRepository = masterFoodRepository;
            _userActivityRepository = userActivityRepository;
        }

        public async Task<BusinessLogicResponse<FoodDiaryEntry>> CreateFoodDiaryEntry(FoodDiaryEntry foodDiaryEntry)
        {
            try
            {
                // TODO: Deal with food -- either retrieve and validate reference to existing master food or create new master food

                var savedEntry = await _foodDiaryEntryRepository.InsertAsync(foodDiaryEntry);
                return new(savedEntry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create food diary entry");
                return new(ErrorType.RuntimeException, "We're sorry.  We were unable to save your food diary entry due to an internal error.  Please try your request again.  If the problem persists, please contact support.");
            }
        }

        public async Task<BusinessLogicResponse<FoodDiaryDailySummary>> GetDailySummary(Guid userId, DateOnly date, string userTimeZone = DefaultTimeZoneId)
        {
            try
            {
                var foodDiaryEntries = await _foodDiaryEntryRepository.GetByUserAndDateAsync(userId, date);

                // TODO: Exercise -- need a method to get collection of UserActivity by user ID and date range

                var result = new FoodDiaryDailySummary(date, foodDiaryEntries, Enumerable.Empty<UserActivity>());
                return new(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create food diary entry");
                return new(ErrorType.RuntimeException, $"We're sorry.  We were unable to retrieve your food diary for {date:d}.  Please try your request again.  If the problem persists, please contact support.");
            }
        }
    }
}
