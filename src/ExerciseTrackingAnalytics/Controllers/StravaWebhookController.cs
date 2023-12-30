using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ExerciseTrackingAnalytics.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;

namespace ExerciseTrackingAnalytics.Controllers
{
    [Route("api/strava-webhook")]
    [ApiController]
    public class StravaWebhookController : ControllerBase
    {
        private readonly ILogger _logger;

        public StravaWebhookController(ILogger<StravaWebhookController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var queryStringParams = HttpContext.Request.Query;
            var formattedQueryParams = string.Join("; ", queryStringParams.Select(qsKvp => string.Format("{0}={1}", qsKvp.Key, qsKvp.Value)));

            _logger.LogDebug("Strava Webhook's GET endpoint was called the the following parameters: {queryStringParams}", formattedQueryParams);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StravaWebhookEventViewModel webhookEvent)
        {
            if (webhookEvent == null)
            {
                return Problem(
                    "POST body did not conform to the expected format and could not be properly parsed",
                    instance: HttpContext.Request.GetDisplayUrl(),
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid POST body",
                    type: "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400");
            }

            _logger.LogDebug("Received Strava Webhook event: {webhookEvent}", JsonSerializer.Serialize(webhookEvent));

            return Ok();
        }
    }
}
