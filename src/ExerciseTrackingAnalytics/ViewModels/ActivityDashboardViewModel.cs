using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.ViewModels
{
    public class ActivityDashboardViewModel
    {
        public ActivityAggregateStatisticsViewModel MonthToDate { get; set; } = new();

        public ActivityAggregateStatisticsViewModel YearToDate { get; set;  } = new();

        public static ActivityDashboardViewModel FromModel(ActivityDashboardData model)
        {
            return new ActivityDashboardViewModel()
            {
                MonthToDate = ActivityAggregateStatisticsViewModel.FromModel(model.MonthToDate!),
                YearToDate = ActivityAggregateStatisticsViewModel.FromModel(model.YearToDate!),
            };
        }
    }
}
