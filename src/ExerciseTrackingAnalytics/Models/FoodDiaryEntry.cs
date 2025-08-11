using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models.Identity;

namespace ExerciseTrackingAnalytics.Models
{
    public class FoodDiaryEntry : IAggregateNutritionalInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }

        [Required]
        public Guid OwnerUserId { get; set; }
        public ApplicationUser? OwnerUser { get; private set; }

        [Required]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        public TimeSpan TimeOfDay { get; set; } = DateTime.Now.TimeOfDay;

        [Required]
        public MealType Meal { get; set; } = MealType.Unspecified;

        [Required]
        public long FoodId { get; set; }
        public MasterFood? Food { get; set; }

        [Required]
        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal NumServings { get; set; } = 1.00m;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "TIMESTAMP")]
        public DateTime RecordInsertDateUtc { get => _RecordInsertDateUtc; private set => _RecordInsertDateUtc = value.AsUtc(); }
        private DateTime _RecordInsertDateUtc;

        [Column(TypeName = "TIMESTAMP")]
        public DateTime? RecordUpdateDateUtc { get => _RecordUpdateDateUtc; set => _RecordUpdateDateUtc = value.AsUtc(); }
        private DateTime? _RecordUpdateDateUtc;

        [NotMapped]
        public string? Portion => Food?.ServingSizeUnit.ToQuantity((double)(NumServings * Food.ServingSize), format: "N1");

        #region IAggregateNutritionalInformation
        [NotMapped]
        public decimal Calories => NumServings * Food?.Calories ?? 0.00m;

        #region Fats
        [NotMapped]
        public decimal? TotalFatGrams => NumServings * Food?.TotalFatGrams;

        [NotMapped]
        public decimal? SaturatedFatGrams => NumServings * Food?.SaturatedFatGrams;

        [NotMapped]
        public decimal? PolyUnsaturatedFatGrams => NumServings * Food?.PolyUnsaturatedFatGrams;

        [NotMapped]
        public decimal? MonoUnsaturatedFatGrams => NumServings * Food?.MonoUnsaturatedFatGrams;

        [NotMapped]
        public decimal? TransFatGrams => NumServings * Food?.TransFatGrams;
        #endregion Fats

        [NotMapped]
        public decimal? CholesterolMilligrams => NumServings * Food?.CholesterolMilligrams;

        [NotMapped]
        public decimal? SodiumMilligrams => NumServings * Food?.SodiumMilligrams;

        #region Carbohydrates
        [NotMapped]
        public decimal? TotalCarbohydratesGrams => NumServings * Food?.TotalCarbohydratesGrams;

        [NotMapped]
        public decimal? DietaryFiberGrams => NumServings * Food?.DietaryFiberGrams;

        [NotMapped]
        public decimal? TotalSugarsGrams => NumServings * Food?.TotalSugarsGrams;

        [NotMapped]
        public decimal? AddedSugarsGrams => NumServings * Food?.AddedSugarsGrams;
        #endregion Carbohydrates

        [NotMapped]
        public decimal? ProteinGrams => NumServings * Food?.ProteinGrams;

        #region Vitamins
        [NotMapped]
        public decimal? VitaminA_Micrograms => NumServings * Food?.VitaminA_Micrograms;

        [NotMapped]
        public decimal? VitaminB6_Micrograms => NumServings * Food?.VitaminB6_Micrograms;

        [NotMapped]
        public decimal? VitaminB12_Micrograms => NumServings * Food?.VitaminB12_Micrograms;

        [NotMapped]
        public decimal? VitaminC_Micrograms => NumServings * Food?.VitaminC_Micrograms;

        [NotMapped]
        public decimal? VitaminD_Micrograms => NumServings * Food?.VitaminD_Micrograms;

        [NotMapped]
        public decimal? VitaminE_Micrograms => NumServings * Food?.VitaminE_Micrograms;

        [NotMapped]
        public decimal? VitaminK_Micrograms => NumServings * Food?.VitaminK_Micrograms;

        [NotMapped]
        public decimal? BiotinMicrograms => NumServings * Food?.BiotinMicrograms;

        [NotMapped]
        public decimal? CholineMicrograms => NumServings * Food?.CholineMicrograms;

        [NotMapped]
        public decimal? FolateMicrograms => NumServings * Food?.FolateMicrograms;

        [NotMapped]
        public decimal? NiacinMicrograms => NumServings * Food?.NiacinMicrograms;

        [NotMapped]
        public decimal? PantothenicAcidMicrograms => NumServings * Food?.PantothenicAcidMicrograms;

        [NotMapped]
        public decimal? RiboflavinMicrograms => NumServings * Food?.RiboflavinMicrograms;

        [NotMapped]
        public decimal? ThiaminMicrograms => NumServings * Food?.ThiaminMicrograms;
        #endregion Vitamins

        #region Minerals
        [NotMapped]
        public decimal? CalciumMilligrams => NumServings * Food?.CalciumMilligrams;

        [NotMapped]
        public decimal? ChlorideMilligrams => NumServings * Food?.ChlorideMilligrams;

        [NotMapped]
        public decimal? ChromiumMicrograms => NumServings * Food?.ChromiumMicrograms;

        [NotMapped]
        public decimal? CopperMicrograms => NumServings * Food?.CopperMicrograms;

        [NotMapped]
        public decimal? IodineMicrograms => NumServings * Food?.IodineMicrograms;

        [NotMapped]
        public decimal? IronMilligrams => NumServings * Food?.IronMilligrams;

        [NotMapped]
        public decimal? MagnesiumMilligrams => NumServings * Food?.MagnesiumMilligrams;

        [NotMapped]
        public decimal? ManganeseMilligrams => NumServings * Food?.ManganeseMilligrams;

        [NotMapped]
        public decimal? MolybdenumMicrograms => NumServings * Food?.MolybdenumMicrograms;

        [NotMapped]
        public decimal? PhosphorusMilligrams => NumServings * Food?.PhosphorusMilligrams;

        [NotMapped]
        public decimal? PotassiumMilligrams => NumServings * Food?.PotassiumMilligrams;

        [NotMapped]
        public decimal? SeleniumMicrograms => NumServings * Food?.SeleniumMicrograms;

        [NotMapped]
        public decimal? ZincMilligrams => NumServings * Food?.ZincMilligrams;
        #endregion Minerals
        #endregion IAggregateNutritionalInformation
    }
}
