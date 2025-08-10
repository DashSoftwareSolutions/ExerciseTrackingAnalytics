using System.ComponentModel.DataAnnotations;

namespace ExerciseTrackingAnalytics.Models
{
    /// <summary>
    /// Abstract base class containing all of the nutritional information properties
    /// </summary>
    /// <remarks>
    /// Derived type can either represent a single food serving, or an aggregation (either a portion consisting of multiple servings of the food, or even a collection of food portions, such a summary of food diary entries for a day or range of days).<br />
    /// <br />
    /// If the derived type represents an aggregation, it should override all getters to do the appropriate aggregate calculation and disallow use of all setters;<br />
    /// all setters should call <see cref="ThrowDoNotSetCalculatedValueException"/>.
    /// </remarks>
    public abstract class NutritionalContent
    {
        // TODO/FIXME: Create an abstract AggregateNutritionalContent that inherits this.
        // Then FoodDiaryEntry and FoodDiaryDailySummary should inherit AggregateNutritionalContent

        [Required]
        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal Calories { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? TotalFatGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? SaturatedFatGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? PolyUnsaturatedFatGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? MonoUnsaturatedFatGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? TransFatGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? CholesterolMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? SodiumMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? PotassiumMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? TotalCarbohydratesGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? DietaryFiberGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? TotalSugarsGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? ProteinGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? VitaminA_MicroGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? VitaminC_MicroGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? VitaminD_MicroGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? CalciumMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? IronMilligrams { get; set; }

        /// <summary>
        /// Use this method to throw an exception for all setters if your derived type represents a computed aggregation
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected static void ThrowDoNotSetCalculatedValueException() => throw new InvalidOperationException("This property is computed; you should not try to set it manually.");
    }
}
