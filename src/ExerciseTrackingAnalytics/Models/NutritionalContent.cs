using System.ComponentModel.DataAnnotations;

namespace ExerciseTrackingAnalytics.Models
{
    public abstract class NutritionalContent
    {
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
    }
}
