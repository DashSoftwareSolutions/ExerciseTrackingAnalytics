namespace ExerciseTrackingAnalytics.Models
{
    public class FoodDiaryDailySummary
    {

        public FoodDiaryDailySummary(DateOnly date, IEnumerable<FoodDiaryEntry> foodDiaryEntries, IEnumerable<UserActivity> exercise)
        {
            Date = date;

            Meals = foodDiaryEntries
                .GroupBy(fde => fde.Meal)
                .ToDictionary(grp => grp.Key, grp => new FoodSummary(grp.Select(fde => fde)));

            FoodSummary = new FoodSummary(foodDiaryEntries);

            Exercise = exercise;
        }

        public DateOnly Date { get; private set; }

        public Dictionary<MealType, FoodSummary> Meals { get; private set; }

        public FoodSummary FoodSummary { get; private set; }

        public IEnumerable<UserActivity> Exercise { get; private set; }

        public decimal FoodCalories => FoodSummary.Calories;

        public decimal ExerciseCalories => Exercise.Sum(e => e.Calories);

        public decimal NetCalories => FoodCalories - ExerciseCalories;
    }
}
