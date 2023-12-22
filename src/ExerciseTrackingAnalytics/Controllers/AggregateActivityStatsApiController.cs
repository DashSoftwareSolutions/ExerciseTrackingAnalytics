using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ExerciseTrackingAnalytics.BusinessLogic;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.ViewModels;
using static ExerciseTrackingAnalytics.Constants;

namespace ExerciseTrackingAnalytics.Controllers
{
    [ApiController]
    [Route("api/activity-statistics")]
    [Authorize]
    public class AggregateActivityStatsApiController : Controller
    {
        private readonly IActivityAggregateStatisticsBusinessLogic _businessLogic;
        private readonly ILogger<AggregateActivityStatsApiController> _logger;

        public AggregateActivityStatsApiController(
            IActivityAggregateStatisticsBusinessLogic businessLogic,
            ILogger<AggregateActivityStatsApiController> logger)
        {
            _businessLogic = businessLogic;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAggregateStatistics([FromQuery] string? tz)
        {
            string specifiedUserTimeZoneId;

            if (string.IsNullOrWhiteSpace(tz))
            {
                _logger.LogWarning("Request did not specify a Time Zone.  Using system default Time Zone '{systemDefaultTimeZoneId}'", DefaultTimeZoneId);
                specifiedUserTimeZoneId = DefaultTimeZoneId;
            }
            else
            {
                specifiedUserTimeZoneId = tz;
            }

            var contextUserId = HttpContext!.User.GetUserId();
            var bizLogicResponse = await _businessLogic.GetActivityAggregateStatisticsAsync(contextUserId, specifiedUserTimeZoneId);

            if (!bizLogicResponse.IsSuccessful)
            {
                var httpResponseStatus = bizLogicResponse.GetHttpStatus();
                var httpResponseStatusNum = (int)httpResponseStatus;

                return Problem(
                    bizLogicResponse.ErrorMessage,
                    instance: HttpContext.Request.GetDisplayUrl(),
                    statusCode: httpResponseStatusNum,
                    title: "Error Fetching Activity Dashboard Data",
                    type: $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{httpResponseStatusNum}");
            }

            return Json(ActivityDashboardViewModel.FromModel(bizLogicResponse.Data!));
        }
    }
}
