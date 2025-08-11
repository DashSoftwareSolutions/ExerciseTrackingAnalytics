using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.ViewModels
{
    public class MasterFoodSummaryResponseViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public string? Barcode { get; set; }
        public decimal Calories { get; set; }
        public decimal ServingSize { get; set; } = 1.00m;
        public string ServingSizeUnit { get; set; } = string.Empty;

        public static MasterFoodSummaryResponseViewModel? FromModel(MasterFood? model)
        {
            if (model == null)
                return null;

            return new MasterFoodSummaryResponseViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Barcode = model.Barcode,
                Brand = model.BrandName,
                Calories = model.Calories,
                ServingSize = model.ServingSize,
                ServingSizeUnit = model.ServingSizeUnit,
            };
        }
    }
}
