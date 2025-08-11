using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ExerciseTrackingAnalytics.BusinessLogic;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.ViewModels;

namespace ExerciseTrackingAnalytics.Controllers
{
    [ApiController]
    [Route("api/master-foods")]
    [Authorize]
    public class MasterFoodApiController : Controller
    {
        private readonly IMasterFoodBusinessLogic _masterFoodBusinessLogic;

        public MasterFoodApiController(IMasterFoodBusinessLogic masterFoodBusinessLogic)
        {
            _masterFoodBusinessLogic = masterFoodBusinessLogic;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? query = null, [FromQuery] string? barcode = null)
        {
            var contextUserId = HttpContext!.User.GetUserId();
            var bizLogicResponse = await _masterFoodBusinessLogic.Search(contextUserId, query, barcode);
            
            if (!bizLogicResponse.IsSuccessful)
            {
                var httpResponseStatus = bizLogicResponse.GetHttpStatus();
                var httpResponseStatusNum = (int)httpResponseStatus;

                return Problem(
                    bizLogicResponse.ErrorMessage,
                    instance: HttpContext.Request.GetDisplayUrl(),
                    statusCode: httpResponseStatusNum,
                    title: "Error Fetching Food Data",
                    type: $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{httpResponseStatusNum}");
            }

            return Ok(bizLogicResponse.Data!.Select(MasterFoodSummaryResponseViewModel.FromModel));
        }
    }
}
