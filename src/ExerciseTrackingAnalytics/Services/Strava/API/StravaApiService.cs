using Newtonsoft.Json;
using ExerciseTrackingAnalytics.Exceptions;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models.Strava;
using StravaAuthentication = ExerciseTrackingAnalytics.Security.Authentication.Strava.Constants;

namespace ExerciseTrackingAnalytics.Services.Strava.API
{
    public class StravaApiService : IStravaApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<StravaApiService> _logger;

        private const string _apiBaseUrl = "https://www.strava.com/api/v3";

        public StravaApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor contextAccessor,
            ILogger<StravaApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        public async Task<IEnumerable<StravaActivity>> GetRecentActivitiesAsync(
            DateTime? beforeDateUtc = null,
            DateTime? afterDateUtc = null,
            int pageNumber = 1,
            int pageSize = 30)
        {
            var accessToken = GetAccessToken();
            var httpClient = _httpClientFactory.CreateClient("Strava");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            try
            {
                var queryParams = new Dictionary<string, string>();

                if (beforeDateUtc.HasValue)
#pragma warning disable CS8604 // Possible null reference argument.
                    queryParams.Add("before", beforeDateUtc.AsUtc().ToUnixTimestamp().ToString());
#pragma warning restore CS8604 // Possible null reference argument.

                if (afterDateUtc.HasValue)
#pragma warning disable CS8604 // Possible null reference argument.
                    queryParams.Add("after", afterDateUtc.AsUtc().ToUnixTimestamp().ToString());
#pragma warning restore CS8604 // Possible null reference argument.

                queryParams.Add("page", pageNumber.ToString());
                queryParams.Add("per_page", pageSize.ToString());

                var requestUrl = $"{_apiBaseUrl}/athlete/activities{queryParams.ToUrlQueryString()}";

                var response = await httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                var responseBodyContent = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<IEnumerable<StravaActivity>>(responseBodyContent);

                return results ?? Enumerable.Empty<StravaActivity>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error encountered while querying Strava for Athlete Recent Activities");
                var status = ex.StatusCode.HasValue ? $"{(int)ex.StatusCode} {ex.StatusCode}" : "<UNKNOWN>";
                throw new ExerciseTrackingAnalyticsAppException($"Failed to query Strava for Athlete Recent Activities due to an API error.  API returned HTTP Response status {status}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-HTTP Error encountered while querying Strava for Athlete Recent Activities");
                throw new ExerciseTrackingAnalyticsAppException("Failed to query Strava for Athlete Recent Activities due to an internal system error.");
            }
        }

        public async Task HydrateActivityCaloriesAsync(StravaActivity activity)
        {
            var accessToken = GetAccessToken();
            var httpClient = _httpClientFactory.CreateClient("Strava");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            try
            {
                var requestUrl = $"{_apiBaseUrl}/activities/{activity.Id}";
                var response = await httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                var responseBodyContent = await response.Content.ReadAsStringAsync();
                var activityDetail = JsonConvert.DeserializeObject<StravaActivity>(responseBodyContent);
                activity.Calories = activityDetail?.Calories ?? default;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Error encountered while querying Strava for details of Activity ID {stravaActivityId}", activity.Id);
                var status = ex.StatusCode.HasValue ? $"{(int)ex.StatusCode} {ex.StatusCode}" : "<UNKNOWN>";
                throw new ExerciseTrackingAnalyticsAppException($"Failed to query Strava for details of Activity ID {activity.Id} due to an API error.  API returned HTTP Response status {status}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-HTTP Error encountered while querying Strava for details of Activity ID {stravaActivityId}", activity.Id);
                throw new ExerciseTrackingAnalyticsAppException($"Failed to query Strava for details of Activity ID {activity.Id} due to an internal system error.");
            }
        }

        private string GetAccessToken()
        {
            var accessToken = _contextAccessor.HttpContext?.User.FindFirst($"{StravaAuthentication.AuthenticationScheme.ToLower()}_access_token")?.Value;

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidOperationException("User access token could not be obtained from the context.  Unable to query Strava for recent activities.");
            }

            return accessToken;
        }
    }
}
