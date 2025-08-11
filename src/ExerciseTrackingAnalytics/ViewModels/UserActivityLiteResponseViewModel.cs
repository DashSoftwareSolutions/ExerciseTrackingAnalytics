using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.ViewModels
{
    public class UserActivityLiteResponseViewModel
    {
        public long Id { get; private set; }
        public long ExternalAppActivityId { get; private set; }

        public ExerciseTrackingApp ExternalApp { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string SportType { get; private set; } = string.Empty;

        public DateTime StartDateUtc { get; private set; }

        public Dictionary<MeasurementSystem, DistanceMeasurementViewModel> Distance { get; private set; } = new();

        public string MovingTime { get; private set; } = "00:00:00";

        public decimal Calories { get; private set; }

        public static UserActivityLiteResponseViewModel? FromModel(UserActivity model)
        {
            if (model == null)
                return null;

            var result = new UserActivityLiteResponseViewModel()
            {
                Id = model.Id,
                ExternalAppActivityId = model.ExternalAppActivityId,
                ExternalApp = model.ExternalApp,
                Name = model.Name,
                SportType = model.SportType,
                StartDateUtc = model.StartDateUtc,
                MovingTime = TimeSpan.FromSeconds(model.MovingTimeInSeconds).ToHoursMinutesSecondsString(),
                Calories = model.Calories,
            };

            result.Distance.Add(MeasurementSystem.UnitedStates, new DistanceMeasurementViewModel() { Amount = model.DistanceInMiles, Unit = DistanceUnit.Miles });
            result.Distance.Add(MeasurementSystem.MetricSystem, new DistanceMeasurementViewModel() { Amount = model.DistanceInMeters / 1000.00m, Unit = DistanceUnit.Kilometers });

            return result;
        }
    }
}
