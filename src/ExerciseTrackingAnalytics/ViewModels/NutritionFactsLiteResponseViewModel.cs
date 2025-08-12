using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.ViewModels
{
    public class NutritionFactsLiteResponseViewModel
    {
        public decimal? TotalFatGrams { get; private set; }

        public decimal? SaturatedFatGrams { get; private set; }

        public decimal? TransFatGrams { get; private set; }

        public decimal? CholesterolMilligrams { get; private set; }

        public decimal? SodiumMilligrams { get; private set; }

        public decimal? TotalCarbohydratesGrams { get; private set; }

        public decimal? DietaryFiberGrams { get; private set; }

        public decimal? TotalSugarsGrams { get; private set; }

        public decimal? AddedSugarsGrams { get; private set; }

        public decimal? ProteinGrams { get; private set; }

        public static NutritionFactsLiteResponseViewModel FromModel(FoodSummary? foodSummary)
        {
            if (foodSummary == null)
                return new NutritionFactsLiteResponseViewModel();

            return new NutritionFactsLiteResponseViewModel()
            {
                TotalFatGrams = foodSummary.TotalFatGrams,
                SaturatedFatGrams = foodSummary.SaturatedFatGrams,
                TransFatGrams = foodSummary.TransFatGrams,
                CholesterolMilligrams = foodSummary.CholesterolMilligrams,
                SodiumMilligrams = foodSummary.SodiumMilligrams,
                TotalCarbohydratesGrams = foodSummary.TotalCarbohydratesGrams,
                DietaryFiberGrams = foodSummary.DietaryFiberGrams,
                TotalSugarsGrams = foodSummary.TotalSugarsGrams,
                AddedSugarsGrams = foodSummary.AddedSugarsGrams,
                ProteinGrams = foodSummary.ProteinGrams,
            };
        }
    }
}
