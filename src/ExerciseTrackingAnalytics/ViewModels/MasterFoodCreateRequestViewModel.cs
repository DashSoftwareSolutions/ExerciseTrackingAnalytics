using System.ComponentModel.DataAnnotations;
using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.ViewModels
{
    public class MasterFoodCreateRequestViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(1024)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string? Brand { get; set; }

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

        [Required]
        public bool IsShared { get; set; } = false;

        public static MasterFood? ToModel(MasterFoodCreateRequestViewModel? viewModel)
        {
            if (viewModel == null)
                return null;

            return new MasterFood()
            {
                Name = viewModel.Name.Trim(),
                BrandName = viewModel.Brand?.Trim(),
                ServingSize = viewModel.ServingSize,
                ServingSizeUnit = viewModel.ServingSizeUnit.Trim(),
                Calories = viewModel.Calories,
                TotalFatGrams = viewModel.TotalFatGrams,
                SaturatedFatGrams = viewModel.SaturatedFatGrams,
                PolyUnsaturatedFatGrams = viewModel.PolyUnsaturatedFatGrams,
                MonoUnsaturatedFatGrams = viewModel.MonoUnsaturatedFatGrams,
                TransFatGrams = viewModel.TransFatGrams,
                CholesterolMilligrams = viewModel.CholesterolMilligrams,
                SodiumMilligrams = viewModel.SodiumMilligrams,
                TotalCarbohydratesGrams = viewModel.TotalCarbohydratesGrams,
                DietaryFiberGrams = viewModel.DietaryFiberGrams,
                TotalSugarsGrams = viewModel.TotalSugarsGrams,
                AddedSugarsGrams = viewModel.AddedSugarsGrams,
                ProteinGrams = viewModel.ProteinGrams,
                VitaminA_Micrograms = viewModel.VitaminA_Micrograms,
                VitaminB6_Micrograms = viewModel.VitaminB6_Micrograms,
                VitaminB12_Micrograms = viewModel.VitaminB12_Micrograms,
                VitaminC_Micrograms = viewModel.VitaminC_Micrograms,
                VitaminD_Micrograms = viewModel.VitaminD_Micrograms,
                VitaminE_Micrograms = viewModel.VitaminE_Micrograms,
                VitaminK_Micrograms = viewModel.VitaminK_Micrograms,
                BiotinMicrograms = viewModel.BiotinMicrograms,
                CholineMicrograms = viewModel.CholineMicrograms,
                FolateMicrograms = viewModel.FolateMicrograms,
                NiacinMicrograms = viewModel.NiacinMicrograms,
                PantothenicAcidMicrograms = viewModel.PantothenicAcidMicrograms,
                RiboflavinMicrograms = viewModel.RiboflavinMicrograms,
                ThiaminMicrograms = viewModel.ThiaminMicrograms,
                CalciumMilligrams = viewModel.CalciumMilligrams,
                ChlorideMilligrams = viewModel.ChlorideMilligrams,
                ChromiumMicrograms = viewModel.ChromiumMicrograms,
                CopperMicrograms = viewModel.CopperMicrograms,
                IodineMicrograms = viewModel.IodineMicrograms,
                MagnesiumMilligrams = viewModel.MagnesiumMilligrams,
                ManganeseMilligrams = viewModel.ManganeseMilligrams,
                MolybdenumMicrograms = viewModel.MolybdenumMicrograms,
                PhosphorusMilligrams = viewModel.PhosphorusMilligrams,
                PotassiumMilligrams = viewModel.PotassiumMilligrams,
                SeleniumMicrograms = viewModel.SeleniumMicrograms,
                ZincMilligrams = viewModel.ZincMilligrams,
            };
        }
    }
}
