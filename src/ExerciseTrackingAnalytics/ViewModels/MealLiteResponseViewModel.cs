using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.ViewModels
{
    public class MealLiteResponseViewModel
    {
        public MealType Meal { get; private set; }

        public decimal TotalCalories => Foods.Sum(f => f.Calories);

        public IEnumerable<FoodDiaryEntryLiteResponseViewModel> Foods { get; private set; } = Enumerable.Empty<FoodDiaryEntryLiteResponseViewModel>();

        public NutritionFactsLiteResponseViewModel Nutrition { get; private set; } = new();

        public static MealLiteResponseViewModel FromModel(MealType meal, FoodSummary mealFoodSummary)
        {
            return new MealLiteResponseViewModel()
            {
                Meal = meal,
                Foods = mealFoodSummary.Foods.Select(FoodDiaryEntryLiteResponseViewModel.FromModel).Where(fde => fde != null).Cast<FoodDiaryEntryLiteResponseViewModel>(),
                Nutrition = NutritionFactsLiteResponseViewModel.FromModel(mealFoodSummary),
            };
        }
    }
}
