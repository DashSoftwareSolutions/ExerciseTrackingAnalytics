using ExerciseTrackingAnalytics.Data.Repositories;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models;
using ExerciseTrackingAnalytics.Services.Time;

namespace ExerciseTrackingAnalytics.BusinessLogic
{
    public class ActivityAggregateStatisticsBusinessLogic : IActivityAggregateStatisticsBusinessLogic
    {
        private readonly IUserActivityRepository _databaseRepository;
        private readonly ILogger<ActivityAggregateStatisticsBusinessLogic> _logger;
        private readonly ITimeProvider _timeProvider;

        public ActivityAggregateStatisticsBusinessLogic(
            IUserActivityRepository databaseRepository,
            ILogger<ActivityAggregateStatisticsBusinessLogic> logger,
            ITimeProvider timeProvider)
        {
            _databaseRepository = databaseRepository;
            _logger = logger;
            _timeProvider = timeProvider;
        }

        public async Task<BusinessLogicResponse<ActivityDashboardData>> GetActivityAggregateStatisticsAsync(Guid userId, string userTimeZone)
        {
            if (!userTimeZone.IsValidTimeZoneId())
                return new(ErrorType.RequestNotValid, $"Time Zone ID '{userTimeZone}' is not a valid IANA Time Zone Database ID (e.g. America/Los_Angeles for U.S. Pacific Time)");

            var utcNow = _timeProvider.UtcNow;
            var localNow = utcNow.ConvertToTimeZoneFromUtc(userTimeZone);

            _logger.LogDebug(
                "Current time is {utcNow:O} UTC / {localNow:O} in Time Zone {timeZone}",
                utcNow,
                localNow,
                userTimeZone);

            var currentMonthStartUtc = new DateTime(localNow.Year, localNow.Month, 1, 0, 0, 0).ConvertToUtcFromTimeZone(userTimeZone);
            var currentYearStartUtc = new DateTime(localNow.Year, 1, 1, 0, 0, 0).ConvertToUtcFromTimeZone(userTimeZone);

            try
            {
                var monthToDateStatsRetrievalTask = _databaseRepository.GetActivityAggregateStatisticsAsync(userId, currentMonthStartUtc, utcNow);
                var yearToDateStatsRetrievalTask = _databaseRepository.GetActivityAggregateStatisticsAsync(userId, currentYearStartUtc, utcNow);
                
                await Task.WhenAll(monthToDateStatsRetrievalTask, yearToDateStatsRetrievalTask);

                var monthToDateStats = await monthToDateStatsRetrievalTask;
                monthToDateStats.NumDays = (int)Math.Ceiling((utcNow - currentMonthStartUtc).TotalDays);

                var yearToDateStats = await yearToDateStatsRetrievalTask;
                yearToDateStats.NumDays = (int)Math.Ceiling((utcNow - currentYearStartUtc).TotalDays);

                var result = new ActivityDashboardData()
                {
                    MonthToDate = monthToDateStats,
                    YearToDate = yearToDateStats,
                };

                return new(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get Activity Aggregate Statistics");
                return new(ErrorType.RuntimeException, "User's activity dashboard data could not be retreived due to a technical error");
            }
        }
    }
}
