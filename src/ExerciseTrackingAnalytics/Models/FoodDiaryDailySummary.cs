using System.Collections.Immutable;

namespace ExerciseTrackingAnalytics.Models
{
    public class FoodDiaryDailySummary : NutritionalContent
    {
        private IEnumerable<FoodDiaryEntry> _allFoods;

        public FoodDiaryDailySummary(DateOnly date, IEnumerable<FoodDiaryEntry> foodDiaryEntries, IEnumerable<UserActivity> exercise)
        {
            _allFoods = foodDiaryEntries;

            Date = date;

            Foods = foodDiaryEntries
                .GroupBy(fde => fde.Meal)
                .ToDictionary(grp => grp.Key, grp => grp.Select(fde => fde).ToImmutableArray().AsEnumerable());

            Exercise = exercise;
        }

        public DateOnly Date { get; private set; }

        public Dictionary<MealType, IEnumerable<FoodDiaryEntry>> Foods { get; private set; }

        public IEnumerable<UserActivity> Exercise { get; private set; }

        public decimal FoodCalories => _allFoods.Sum(f => f.Calories);

        public decimal ExerciseCalories => Exercise.Sum(e => e.Calories);

        public decimal NetCalories => FoodCalories - ExerciseCalories;

        public override decimal Calories { get => NetCalories; set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? TotalFatGrams { get => _allFoods.Sum(f => f.TotalFatGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? SaturatedFatGrams { get => _allFoods.Sum(f => f.SaturatedFatGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? PolyUnsaturatedFatGrams { get => _allFoods.Sum(f => f.PolyUnsaturatedFatGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? MonoUnsaturatedFatGrams { get => _allFoods.Sum(f => f.MonoUnsaturatedFatGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? TransFatGrams { get => _allFoods.Sum(f => f.TransFatGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? CholesterolMilligrams { get => _allFoods.Sum(f => f.CholesterolMilligrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? SodiumMilligrams { get => _allFoods.Sum(f => f.SodiumMilligrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? PotassiumMilligrams { get => _allFoods.Sum(f => f.PotassiumMilligrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? TotalCarbohydratesGrams { get => _allFoods.Sum(f => f.TotalCarbohydratesGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? DietaryFiberGrams { get => _allFoods.Sum(f => f.DietaryFiberGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? TotalSugarsGrams { get => _allFoods.Sum(f => f.TotalSugarsGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? ProteinGrams { get => _allFoods.Sum(f => f.ProteinGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? VitaminA_MicroGrams { get => _allFoods.Sum(f => f.VitaminA_MicroGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? VitaminC_MicroGrams { get => _allFoods.Sum(f => f.VitaminC_MicroGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? VitaminD_MicroGrams { get => _allFoods.Sum(f => f.VitaminD_MicroGrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? CalciumMilligrams { get => _allFoods.Sum(f => f.CalciumMilligrams); set => ThrowDoNotSetCalculatedValueException(); }

        public override decimal? IronMilligrams { get => _allFoods.Sum(f => f.IronMilligrams); set => ThrowDoNotSetCalculatedValueException(); }
    }
}
