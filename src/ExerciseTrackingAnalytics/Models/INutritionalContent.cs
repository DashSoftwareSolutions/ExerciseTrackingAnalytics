using System.ComponentModel.DataAnnotations;

namespace ExerciseTrackingAnalytics.Models
{
    /// <summary>
    /// Interface for an object containing nutritional facts about a serving of a particular food, a portion consisting of one or more servings of a food, or an aggregate (a meal or several meals for a day or range of days)
    /// </summary>
    public interface INutritionalContent
    {
        decimal Calories { get; }

        #region Fats
        decimal? TotalFatGrams { get; }

        decimal? SaturatedFatGrams { get; }

        decimal? PolyUnsaturatedFatGrams { get; }

        decimal? MonoUnsaturatedFatGrams { get; }

        decimal? TransFatGrams { get; }
        #endregion Fats

        decimal? CholesterolMilligrams { get; }

        decimal? SodiumMilligrams { get; }

        #region Carbohydrates
        decimal? TotalCarbohydratesGrams { get; }

        decimal? DietaryFiberGrams { get; }

        decimal? TotalSugarsGrams { get; }

        decimal? AddedSugarsGrams { get; }
        #endregion Carbohydrates

        decimal? ProteinGrams { get; }

        #region Vitamins
        decimal? VitaminA_Micrograms { get; }

        decimal? VitaminB6_Micrograms { get; }

        decimal? VitaminB12_Micrograms { get; }

        decimal? VitaminC_Micrograms { get; }

        decimal? VitaminD_Micrograms { get; }

        decimal? VitaminE_Micrograms { get; }

        decimal? VitaminK_Micrograms { get; }

        decimal? BiotinMicrograms { get; }

        decimal? CholineMicrograms { get; }

        decimal? FolateMicrograms { get; }

        decimal? NiacinMicrograms { get; }

        decimal? PantothenicAcidMicrograms { get; }

        decimal? RiboflavinMicrograms { get; }

        decimal? ThiaminMicrograms { get; }
        #endregion Vitamins

        #region Minerals
        decimal? CalciumMilligrams { get; }

        decimal? ChlorideMilligrams { get; }

        decimal? ChromiumMicrograms { get; }

        decimal? CopperMicrograms {  get; }

        decimal? IodineMicrograms { get; }

        decimal? IronMilligrams { get; }

        decimal? MagnesiumMilligrams { get; }

        decimal? ManganeseMilligrams { get; }

        decimal? MolybdenumMicrograms { get; }

        decimal? PhosphorusMilligrams { get; }

        decimal? PotassiumMilligrams { get; }

        decimal? SeleniumMicrograms { get; }

        decimal? ZincMilligrams { get; }
        #endregion Minerals
    }
}
