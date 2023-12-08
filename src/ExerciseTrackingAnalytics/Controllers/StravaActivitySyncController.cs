using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ExerciseTrackingAnalytics.Services.Strava.ActivitySync;

namespace ExerciseTrackingAnalytics.Controllers
{
    [ApiController]
    [Route("api/strava-activity-sync")]
    [Authorize]
    public class StravaActivitySyncController : Controller
    {
        private readonly IStravaActivitySyncService _syncService;

        public StravaActivitySyncController(IStravaActivitySyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpPost]
        public async Task<IActionResult> SyncStravaActivities()
        {
            var result = await _syncService.SyncRecentActivitiesAsync();

            if (result.IsSuccessful)
            {
                var message = result.NumSyncedActivities == 0
                    ? "All recent Strava Activities have been synced to the database"
                    : $"{result.NumSyncedActivities} recent Strava Activities successfully synced";

                return Json(new { Message = message });
            }

            return Problem(
                "We were unable to sync recent Strava Activities due to an unexpected system error.",
                instance: HttpContext.Request.GetDisplayUrl(),
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Error Syncing Recent Strava Activities",
                type: "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500");
        }
    }
}
