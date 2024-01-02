using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ExerciseTrackingAnalytics.Models.Strava;
using ExerciseTrackingAnalytics.ViewModels;

namespace ExerciseTrackingAnalytics.Controllers
{
    [Route("api/strava-webhook")]
    [ApiController]
    public class StravaWebhookController : Controller
    {
        private readonly ILogger _logger;
        private readonly StravaWebhookOptions _options;

        public StravaWebhookController(
            ILogger<StravaWebhookController> logger,
            IOptions<StravaWebhookOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        /// <summary>
        /// GET endpoint to verify a Webhook Subscription.<br />
        /// See <see href="https://developers.strava.com/docs/webhooks/#subscription-validation-request"/> for details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var queryStringParams = HttpContext.Request.Query;
            var formattedQueryParams = string.Join("; ", queryStringParams.Select(qsKvp => string.Format("{0}={1}", qsKvp.Key, qsKvp.Value)));

            _logger.LogDebug("Strava Webhook's GET endpoint was called the the following parameters: {queryStringParams}", formattedQueryParams);

            var subscriptionRegistrationChallenge = queryStringParams["hub.challenge"].ToString();
            var requestVerificationToken = queryStringParams["hub.verify_token"].ToString();

            if (string.IsNullOrEmpty(subscriptionRegistrationChallenge))
            {
                return Problem(
                    "Expected request parameter 'hub.challenge' was not included in the request.",
                    instance: HttpContext.Request.GetDisplayUrl(),
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid Request",
                    type: "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400");
            }

            if (string.IsNullOrEmpty(requestVerificationToken))
            {
                return Problem(
                    "Expected request parameter 'hub.verify_token' was not included in the request.",
                    instance: HttpContext.Request.GetDisplayUrl(),
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid Request",
                    type: "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400");
            }

            if (requestVerificationToken != _options.VerificationToken)
            {
                return Problem(
                    "Invalid value supplied in request parameter 'hub.verify_token'.  Request is not authorized.",
                    instance: HttpContext.Request.GetDisplayUrl(),
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized",
                    type: "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/401");
            }

            var result = new Dictionary<string, string>() { { "hub.challenge", subscriptionRegistrationChallenge } };
            return Json(result);
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
