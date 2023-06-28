using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ExerciseTrackingAnalytics.Services;

namespace ExerciseTrackingAnalytics.Controllers
{
    [ApiController]
    [Route("api/strava-activity-sync")]
    [Authorize]
    public class StravaActivitySyncController : Controller
    {
        private readonly ILogger<StravaActivitySyncController> _logger;
        private readonly IStravaApiService _stravaApiService;

        public StravaActivitySyncController(
            ILogger<StravaActivitySyncController> logger,
            IStravaApiService stravaApiService)
        {
            _logger = logger;
            _stravaApiService = stravaApiService;
        }

        [HttpPost]
        public async Task<IActionResult> SyncStravaActivities()
        {
            _logger.LogInformation("Syncing Strava Activities...");

            try
            {
                var results = await _stravaApiService.GetRecentActivitiesAsync();
                _logger.LogInformation("Found {count} recent activities", results.Count());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing Strava Activities");

                return Problem(
                    "We were unable to sync recent Strava Activities due to an unexpected system error.",
                    instance: HttpContext.Request.GetDisplayUrl(),
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Error Syncing Recent Strava Activities",
                    type: "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500");
            }
        }
    }
}
