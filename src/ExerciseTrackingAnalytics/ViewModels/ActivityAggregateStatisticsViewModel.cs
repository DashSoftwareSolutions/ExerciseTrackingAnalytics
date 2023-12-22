using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.ViewModels
{
    public class ActivityAggregateStatisticsViewModel
    {
        public int NumActivities { get; set; }

        public int NumDays { get; set; }

        public Dictionary<MeasurementSystem, DistanceMeasurementViewModel> TotalDistance { get; set; } = new();

        public string TotalElapsedTime { get; set; } = "00:00:00";

        public string TotalMovingTime { get; set; } = "00:00:00";

        public decimal TotalCalories { get; set; }

        public Dictionary<MeasurementSystem, SpeedMeasurementViewModel> AverageSpeed { get; set; } = new();

        public Dictionary<MeasurementSystem, DistanceMeasurementViewModel> AverageDistancePerDay { get; set; } = new();

        public static ActivityAggregateStatisticsViewModel FromModel(ActivityAggregateStatistics model)
        {
            var elapsedTime = TimeSpan.FromSeconds(model.TotalElapsedTimeInSeconds);
            var movingTime = TimeSpan.FromSeconds(model.TotalMovingTimeInSeconds);

            var hasAverageSpeedMetric = model.AverageKilometersPerHour.HasValue;
            var hasAverageSpeedUS = model.AverageMilesPerHour.HasValue;
            var averageSpeeds = new Dictionary<MeasurementSystem, SpeedMeasurementViewModel>();

            if (hasAverageSpeedMetric)
                averageSpeeds.Add(
                    MeasurementSystem.MetricSystem,
                    new SpeedMeasurementViewModel()
                    {
                        Amount = model.AverageKilometersPerHour!.Value,
                        Unit = SpeedUnit.KilometersPerHour,
                    });

            if (hasAverageSpeedUS)
                averageSpeeds.Add(
                    MeasurementSystem.UnitedStates,
                    new SpeedMeasurementViewModel()
                    {
                        Amount = model.AverageMilesPerHour!.Value,
                        Unit = SpeedUnit.MilesPerHour,
                    });

            var averageDistances = new Dictionary<MeasurementSystem, DistanceMeasurementViewModel>();

            if (model.NumDays != 0) // avoiding divide by zero
            {
                averageDistances.Add(
                    MeasurementSystem.MetricSystem,
                    new DistanceMeasurementViewModel()
                    {
                        Amount = (model.TotalDistanceInMeters / 1000m) / model.NumDays,
                        Unit = DistanceUnit.Kilometers,
                    });

                averageDistances.Add(
                    MeasurementSystem.UnitedStates,
                    new DistanceMeasurementViewModel()
                    {
                        Amount = model.TotalDistanceInMiles / model.NumDays,
                        Unit = DistanceUnit.Miles,
                    });
            }

            return new ActivityAggregateStatisticsViewModel()
            {
                NumActivities = model.NumActivities,
                NumDays = model.NumDays,
                TotalCalories = model.TotalCalories,
                TotalDistance = new Dictionary<MeasurementSystem, DistanceMeasurementViewModel>()
                {
                    {
                        MeasurementSystem.MetricSystem,
                        new DistanceMeasurementViewModel()
                        {
                            Amount = model.TotalDistanceInMeters / 1000.00m,
                            Unit = DistanceUnit.Kilometers,
                        }
                    },
                    {
                        MeasurementSystem.UnitedStates,
                        new DistanceMeasurementViewModel()
                        {
                            Amount = model.TotalDistanceInMiles,
                            Unit = DistanceUnit.Miles,
                        }
                    },
                },
                TotalElapsedTime = elapsedTime.ToHoursMinutesSecondsString(),
                TotalMovingTime = movingTime.ToHoursMinutesSecondsString(),
                AverageSpeed = averageSpeeds,
                AverageDistancePerDay = averageDistances,
            };
        }
    }
}
