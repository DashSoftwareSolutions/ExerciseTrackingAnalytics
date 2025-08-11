using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ExerciseTrackingAnalytics.BusinessLogic;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.ViewModels;

namespace ExerciseTrackingAnalytics.Controllers
{
    [ApiController]
    [Route("api/food-diary")]
    [Authorize]
    public class FoodDiaryApiController : Controller
    {
        private readonly IFoodDiaryBusinessLogic _foodDiaryBusinessLogic;
        private readonly ILogger<FoodDiaryApiController> _logger;

        public FoodDiaryApiController(IFoodDiaryBusinessLogic foodDiaryBusinessLogic, ILogger<FoodDiaryApiController> logger)
        {
            _foodDiaryBusinessLogic = foodDiaryBusinessLogic;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostEntry([FromBody] FoodDiaryEntryCreateRequestViewModel viewModel)
        {
            if (viewModel == null)
            {
                return Problem(
                    "Request body could not be parsed.",
                    instance: HttpContext.Request.GetDisplayUrl(),
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Error Adding Food Diary Entry",
                    type: $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{StatusCodes.Status400BadRequest}"
                );
            }

            var contextUserId = HttpContext!.User.GetUserId();
            
            var foodDiaryEntry = FoodDiaryEntryCreateRequestViewModel.ToModel(viewModel);
            foodDiaryEntry!.OwnerUserId = contextUserId;

            var bizLogicResponse = await _foodDiaryBusinessLogic.CreateFoodDiaryEntry(foodDiaryEntry!);

            if (!bizLogicResponse.IsSuccessful)
            {
                var httpResponseStatus = bizLogicResponse.GetHttpStatus();
                var httpResponseStatusNum = (int)httpResponseStatus;

                return Problem(
                    bizLogicResponse.ErrorMessage,
                    instance: HttpContext.Request.GetDisplayUrl(),
                    statusCode: httpResponseStatusNum,
                    title: "Error Adding Food Diary Entry",
                    type: $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{httpResponseStatusNum}");
            }

            return Accepted();
        }
    }
}
