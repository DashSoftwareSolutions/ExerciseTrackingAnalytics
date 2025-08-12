using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.ViewModels
{
    public class FoodDiaryEntryLiteResponseViewModel
    {
        public long Id { get; private set; }

        public TimeSpan TimeOfDay { get; private set; }

        public long FoodId { get; private set; }

        public string Food { get; private set; } = string.Empty;
        
        public decimal NumServings { get; private set; }

        public decimal ServingSize { get; private set; }

        public string ServingSizeUnit { get; private set; } = string.Empty;

        public string Portion { get; private set; } = string.Empty;

        public decimal Calories { get; private set; }

        public static FoodDiaryEntryLiteResponseViewModel? FromModel(FoodDiaryEntry? model)
        {
            if (model == null || model.Food == null)
                return null;

            return new FoodDiaryEntryLiteResponseViewModel()
            {
                Id = model.Id,
                TimeOfDay = model.TimeOfDay,
                FoodId = model.FoodId,
                Food = model.Food.Name,
                NumServings = model.NumServings,
                ServingSize = model.Food.ServingSize,
                ServingSizeUnit = model.Food.ServingSizeUnit,
                Portion = model.Portion ?? string.Empty,
                Calories = model.Calories,
            };
        }
    }
}
