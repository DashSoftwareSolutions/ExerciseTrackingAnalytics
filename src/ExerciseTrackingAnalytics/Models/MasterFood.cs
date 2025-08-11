using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models.Identity;

namespace ExerciseTrackingAnalytics.Models
{
    public class MasterFood : INutritionalContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(1024)]
        public string Name { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [MaxLength(2048)]
        public string NameNormalized { get => _NameNormalized ?? Name.ToLowerInvariant(); private set => _NameNormalized = value; }
        private string? _NameNormalized;

        [Required]
        public int Version { get; set; } = 1;

        [MaxLength(1024)]
        public string? BrandName { get; set; }

        [Required]
        [Range(typeof(decimal), "1.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal ServingSize { get; set; } = 1.00m;

        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string ServingSizeUnit { get; set; } = string.Empty;

        #region Nutritional Content
        [Required]
        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal Calories { get; set; }

        #region Fats
        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? TotalFatGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? SaturatedFatGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? PolyUnsaturatedFatGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? MonoUnsaturatedFatGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? TransFatGrams { get; set; }
        #endregion Fats

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? CholesterolMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? SodiumMilligrams { get; set; }

        #region Carbohydrates
        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? TotalCarbohydratesGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? DietaryFiberGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? TotalSugarsGrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? AddedSugarsGrams { get; set; }
        #endregion Carbohydrates

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? ProteinGrams { get; set; }

        #region Vitamins
        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? VitaminA_Micrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? VitaminB6_Micrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? VitaminB12_Micrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? VitaminC_Micrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? VitaminD_Micrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? VitaminE_Micrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? VitaminK_Micrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? BiotinMicrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? CholineMicrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? FolateMicrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? NiacinMicrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? PantothenicAcidMicrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? RiboflavinMicrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? ThiaminMicrograms { get; set; }
        #endregion Vitamins

        #region Minerals
        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? CalciumMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? ChlorideMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? ChromiumMicrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? CopperMicrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? IodineMicrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? IronMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? MagnesiumMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? ManganeseMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? MolybdenumMicrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? PhosphorusMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? PotassiumMilligrams { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? SeleniumMicrograms { get; set; }

        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal? ZincMilligrams { get; set; }
        #endregion Minerals
        #endregion Nutritional Content

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "TIMESTAMP")]
        public DateTime RecordInsertDateUtc { get => _RecordInsertDateUtc; private set => _RecordInsertDateUtc = value.AsUtc(); }
        private DateTime _RecordInsertDateUtc;

        public Guid CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; private set; }

        public Guid? OwnerUserId { get; set; }
        public ApplicationUser? OwnerUser { get; private set; }

        [Required]
        public bool IsShared { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [MaxLength(32)]
        public string? Barcode { get; set; }

        [MaxLength(64)]
        public string? BarcodeNormalized { get => _BarcodeNormalized ?? Barcode?.ToLowerInvariant(); set => _BarcodeNormalized = value; }
        private string? _BarcodeNormalized;

        [Column(TypeName = "TIMESTAMP")]
        public DateTime? DeactivatedDateUtc { get => _DeactivatedDateUtc; private set => _DeactivatedDateUtc = value.AsUtc(); }
        private DateTime? _DeactivatedDateUtc;

        public Guid? DeactivatedByUserId { get; set; }
        public ApplicationUser? DeactivatedByUser { get; private set; }

        public long? PredecessorId { get; set; }
        public MasterFood? Predecessor { get; private set; }
    }
}
