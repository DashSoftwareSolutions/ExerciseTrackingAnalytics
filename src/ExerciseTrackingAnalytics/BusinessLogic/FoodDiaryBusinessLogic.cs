using NodaTime;
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

                var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(userTimeZone);

                if (timeZone == null)
                    return new(ErrorType.RequestNotValid, $"Time Zone ID '{userTimeZone}' does not seem to be a valid IANA / Olson / Tzdb Time Zone identifier (e.g. 'America/Los_Angeles' for the United States Pacific Time Zone).");

                var localStartOfDay = date.ToDateTime(new TimeOnly(0, 0));
                var localEndOfDay = localStartOfDay.AddDays(1).Subtract(TimeSpan.FromMilliseconds(1));
                var exerciseQueryStartDateUtc = LocalDateTime.FromDateTime(localStartOfDay).InZoneLeniently(timeZone);
                var exerciseQueryEndDateUtc = LocalDateTime.FromDateTime(localEndOfDay).InZoneLeniently(timeZone);

                var exerciseEntries = await _userActivityRepository.GetByUserAndDateRange(
                    userId,
                    DateTime.SpecifyKind(exerciseQueryStartDateUtc.ToDateTimeUtc(), DateTimeKind.Unspecified),
                    DateTime.SpecifyKind(exerciseQueryEndDateUtc.ToDateTimeUtc(), DateTimeKind.Unspecified));

                var result = new FoodDiaryDailySummary(date, foodDiaryEntries, exerciseEntries);
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
