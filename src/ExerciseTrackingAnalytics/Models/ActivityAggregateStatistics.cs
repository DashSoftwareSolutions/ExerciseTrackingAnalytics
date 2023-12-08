namespace ExerciseTrackingAnalytics.Models
{
    public class ActivityAggregateStatistics
    {
        /// <summary>
        /// Count of activities in the grouping for these aggregate statistics
        /// </summary>
        public int NumActivities { get; set; }

        public int NumDays { get; set; }

        public decimal TotalDistanceInMeters { get; set; }

        public decimal TotalDistanceInMiles { get; set; }

        public decimal TotalElevationGainInMeters { get; set; }

        public int TotalElapsedTimeInSeconds { get; set; }

        public int TotalMovingTimeInSeconds { get; set; }

        public decimal TotalCalories { get; set; }

        public decimal? AverageMetersPerDay { get => NumDays == 0m ? null : TotalDistanceInMeters / NumDays; }

        public decimal? AverageMilesPerDay { get => NumDays == 0m ? null : TotalDistanceInMiles / NumDays; }

        public decimal? AverageKilometersPerHour { get => TotalMovingTimeInSeconds == 0m ? null : (TotalDistanceInMeters / 1000m) / (TotalMovingTimeInSeconds / 3600m); }

        public decimal? AverageMilesPerHour { get => TotalMovingTimeInSeconds == 0m ? null : TotalDistanceInMiles / (TotalMovingTimeInSeconds / 3600m); }
    }
}
