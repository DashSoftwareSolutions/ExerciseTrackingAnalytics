namespace ExerciseTrackingAnalytics.Models
{
    public class FoodSummary : IAggregateNutritionalInformation
    {
        protected readonly IEnumerable<FoodDiaryEntry> _allFoods;

        public FoodSummary(IEnumerable<FoodDiaryEntry> allFoods)
        {
            _allFoods = allFoods;
        }

        public IEnumerable<FoodDiaryEntry> Foods => _allFoods;

        #region IAggregateNutritionalInformation
        public decimal Calories => _allFoods.Sum(f => f.Calories);

        #region Fats
        public decimal? TotalFatGrams => _allFoods.Sum(f => f.TotalFatGrams);

        public decimal? SaturatedFatGrams => _allFoods.Sum(f => f.SaturatedFatGrams);

        public decimal? PolyUnsaturatedFatGrams => _allFoods.Sum(f => f.PolyUnsaturatedFatGrams);

        public decimal? MonoUnsaturatedFatGrams => _allFoods.Sum(f => f.MonoUnsaturatedFatGrams);

        public decimal? TransFatGrams => _allFoods.Sum(f => f.TransFatGrams);
        #endregion Fats

        public decimal? CholesterolMilligrams => _allFoods.Sum(f => f.CholesterolMilligrams);

        public decimal? SodiumMilligrams => _allFoods.Sum(f => f.SodiumMilligrams);

        #region Carbohydrates
        public decimal? TotalCarbohydratesGrams => _allFoods.Sum(f => f.TotalCarbohydratesGrams);

        public decimal? DietaryFiberGrams => _allFoods.Sum(f => f.DietaryFiberGrams);

        public decimal? TotalSugarsGrams => _allFoods.Sum(f => f.TotalSugarsGrams);

        public decimal? AddedSugarsGrams => _allFoods.Sum(f => f.AddedSugarsGrams);
        #endregion Carbohydrates

        public decimal? ProteinGrams => _allFoods.Sum(f => f.ProteinGrams);

        #region Vitamins
        public decimal? VitaminA_Micrograms => _allFoods.Sum(f => f.VitaminA_Micrograms);

        public decimal? VitaminB6_Micrograms => _allFoods.Sum(f => f.VitaminB6_Micrograms);

        public decimal? VitaminB12_Micrograms => _allFoods.Sum(f => f.VitaminB12_Micrograms);

        public decimal? VitaminC_Micrograms => _allFoods.Sum(f => f.VitaminC_Micrograms);

        public decimal? VitaminD_Micrograms => _allFoods.Sum(f => f.VitaminD_Micrograms);

        public decimal? VitaminE_Micrograms => _allFoods.Sum(f => f.VitaminE_Micrograms);

        public decimal? VitaminK_Micrograms => _allFoods.Sum(f => f.VitaminK_Micrograms);

        public decimal? BiotinMicrograms => _allFoods.Sum(f => f.BiotinMicrograms);

        public decimal? CholineMicrograms => _allFoods.Sum(f => f.CholineMicrograms);

        public decimal? FolateMicrograms => _allFoods.Sum(f => f.FolateMicrograms);

        public decimal? NiacinMicrograms => _allFoods.Sum(f => f.NiacinMicrograms);

        public decimal? PantothenicAcidMicrograms => _allFoods.Sum(f => f.PantothenicAcidMicrograms);

        public decimal? RiboflavinMicrograms => _allFoods.Sum(f => f.RiboflavinMicrograms);

        public decimal? ThiaminMicrograms => _allFoods.Sum(f => f.ThiaminMicrograms);

        #endregion Vitamins

        #region Minerals
        public decimal? CalciumMilligrams => _allFoods.Sum(f => f.CalciumMilligrams);

        public decimal? ChlorideMilligrams => _allFoods.Sum(f => f.ChlorideMilligrams);

        public decimal? ChromiumMicrograms => _allFoods.Sum(f => f.ChromiumMicrograms);

        public decimal? CopperMicrograms => _allFoods.Sum(f => f.CopperMicrograms);

        public decimal? IodineMicrograms => _allFoods.Sum(f => f.IodineMicrograms);

        public decimal? IronMilligrams => _allFoods.Sum(f => f.IronMilligrams);

        public decimal? MagnesiumMilligrams => _allFoods.Sum(f => f.MagnesiumMilligrams);

        public decimal? ManganeseMilligrams => _allFoods.Sum(f => f.ManganeseMilligrams);

        public decimal? MolybdenumMicrograms => _allFoods.Sum(f => f.MolybdenumMicrograms);

        public decimal? PhosphorusMilligrams => _allFoods.Sum(f => f.PhosphorusMilligrams);

        public decimal? PotassiumMilligrams => _allFoods.Sum(f => f.PotassiumMilligrams);

        public decimal? SeleniumMicrograms => _allFoods.Sum(f => f.SeleniumMicrograms);

        public decimal? ZincMilligrams => _allFoods.Sum(f => f.ZincMilligrams);
        #endregion Minerals
        #endregion IAggregateNutritionalInformation
    }
}
