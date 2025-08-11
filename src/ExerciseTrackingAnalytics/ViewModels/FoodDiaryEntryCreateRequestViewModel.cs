using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ExerciseTrackingAnalytics.Models;
using ExerciseTrackingAnalytics.ViewModels.Serialization;

namespace ExerciseTrackingAnalytics.ViewModels
{
    public class FoodDiaryEntryCreateRequestViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateOnlyConverter))]
        public DateOnly Date { get; set; }

        [Required]
        public TimeSpan TimeOfDay { get; set; }

        [Required]
        public MealType Meal { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "{0} must be a positive whole number")]
        public long? FoodId { get; set; }

        public MasterFoodCreateRequestViewModel? Food { get; set; }

        [Required]
        [Range(typeof(decimal), "0.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal NumServings { get; set; } = 1.00m;

        public static FoodDiaryEntry? ToModel(FoodDiaryEntryCreateRequestViewModel viewModel)
        {
            if (viewModel == null)
                return null;

            return new FoodDiaryEntry()
            {
                Date = viewModel.Date,
                TimeOfDay = viewModel.TimeOfDay,
                Meal = viewModel.Meal,
                FoodId = viewModel.FoodId ?? 0,
                Food = MasterFoodCreateRequestViewModel.ToModel(viewModel.Food),
                NumServings = viewModel.NumServings,
            };
        }
    }
}
