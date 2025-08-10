using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models.Identity;

namespace ExerciseTrackingAnalytics.Models
{
    public class FoodDiaryEntry : NutritionalContent
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

        [NotMapped]
        public override decimal Calories { get => NumServings * Food?.Calories ?? 0.00m; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? TotalFatGrams { get => NumServings * Food?.TotalFatGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? SaturatedFatGrams { get => NumServings * Food?.SaturatedFatGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? PolyUnsaturatedFatGrams { get => NumServings * Food?.PolyUnsaturatedFatGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? MonoUnsaturatedFatGrams { get => NumServings * Food?.MonoUnsaturatedFatGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? TransFatGrams { get => NumServings * Food?.TransFatGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? CholesterolMilligrams { get => NumServings * Food?.CholesterolMilligrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? SodiumMilligrams { get => NumServings * Food?.SodiumMilligrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? PotassiumMilligrams { get => NumServings * Food?.PotassiumMilligrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? TotalCarbohydratesGrams { get => NumServings * Food?.TotalCarbohydratesGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? DietaryFiberGrams { get => NumServings * Food?.DietaryFiberGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? TotalSugarsGrams { get => NumServings * Food?.TotalSugarsGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? ProteinGrams { get => NumServings * Food?.ProteinGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? VitaminA_MicroGrams { get => NumServings * Food?.VitaminA_MicroGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? VitaminC_MicroGrams { get => NumServings * Food?.VitaminC_MicroGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? VitaminD_MicroGrams { get => NumServings * Food?.VitaminD_MicroGrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? CalciumMilligrams { get => NumServings * Food?.CalciumMilligrams; set => ThrowDoNotSetCalculatedValueException(); }

        [NotMapped]
        public override decimal? IronMilligrams { get => NumServings * Food?.IronMilligrams; set => ThrowDoNotSetCalculatedValueException(); }

        private static void ThrowDoNotSetCalculatedValueException() => throw new InvalidOperationException("This property is computed; you should not try to set it manually.");
    }
}
