namespace ExerciseTrackingAnalytics.Models
{
    public class ActivityDashboardData
    {
        public ActivityAggregateStatistics? YearToDate { get; set; }

        public ActivityAggregateStatistics? MonthToDate { get; set; }
    }
}
