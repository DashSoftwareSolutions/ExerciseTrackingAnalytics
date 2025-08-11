using System.Text.Json.Serialization;
using ExerciseTrackingAnalytics.Models;
using ExerciseTrackingAnalytics.ViewModels.Serialization;

namespace ExerciseTrackingAnalytics.ViewModels
{
    public class FoodDiaryDailySummaryResponseViewModel
    {
        [JsonConverter(typeof(JsonDateOnlyConverter))]
        public DateOnly Date { get; private set; }

        public decimal FoodCalories { get; private set; }

        public decimal ExerciseCalories { get; private set; }

        public decimal NetCalories { get; private set; }

        public IEnumerable<UserActivityLiteResponseViewModel> Exercise {  get; private set; } = Enumerable.Empty<UserActivityLiteResponseViewModel>();

        public static FoodDiaryDailySummaryResponseViewModel? FromModel(FoodDiaryDailySummary model)
        {
            if (model == null)
                return null;

            return new FoodDiaryDailySummaryResponseViewModel()
            {
                Date = model.Date,
                FoodCalories = model.FoodCalories,
                ExerciseCalories = model.ExerciseCalories,
                NetCalories = model.NetCalories,
                Exercise = model.Exercise.Select(UserActivityLiteResponseViewModel.FromModel).Where(x => x != null).Cast<UserActivityLiteResponseViewModel>(),
            };
        }
    }
}
