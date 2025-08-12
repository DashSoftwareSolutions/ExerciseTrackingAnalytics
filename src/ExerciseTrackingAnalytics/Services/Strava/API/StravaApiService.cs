using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ExerciseTrackingAnalytics.Exceptions;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models.Strava;
using ExerciseTrackingAnalytics.Security.Authentication;
using ExerciseTrackingAnalytics.Security.Authentication.Strava;
using static ExerciseTrackingAnalytics.Services.Strava.API.Constants;
using GlobalConstants = ExerciseTrackingAnalytics.Constants;
using StravaAuthentication = ExerciseTrackingAnalytics.Security.Authentication.Strava.Constants;

namespace ExerciseTrackingAnalytics.Services.Strava.API
{
    public class StravaApiService : IStravaApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<StravaApiService> _logger;
        private readonly StravaOAuthOptions _stravaOptions;
        private readonly ApplicationUserManager _userManager;

        public StravaApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor contextAccessor,
            ILogger<StravaApiService> logger,
            IOptions<StravaOAuthOptions> stravaOptions,
            ApplicationUserManager userManager)
        {
            _httpClientFactory = httpClientFactory;
            _contextAccessor = contextAccessor;
            _logger = logger;
            _stravaOptions = stravaOptions.Value;
            _userManager = userManager;
        }

        public async Task<IEnumerable<StravaActivity>> GetRecentActivitiesAsync(
            DateTime? beforeDateUtc = null,
            DateTime? afterDateUtc = null,
            int pageNumber = 1,
            int pageSize = 30)
        {
            var accessToken = await GetAccessToken();
            _logger.LogDebug("Access Token: {accessToken}", accessToken);

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

                var requestUrl = $"{ApiBaseUrl}/athlete/activities{queryParams.ToUrlQueryString()}";

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
            var accessToken = await GetAccessToken();
            var httpClient = _httpClientFactory.CreateClient("Strava");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            try
            {
                var requestUrl = $"{ApiBaseUrl}/activities/{activity.Id}";
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

        private async Task<string> GetAccessToken()
        {
            if (_contextAccessor.HttpContext == null)
                throw new InvalidOperationException("The Http Context was null");

            var userId = _contextAccessor.HttpContext.User.GetUserId();
            var tokens = await _userManager.GetTokensAsync(userId, StravaAuthentication.AuthenticationScheme);

            var accessToken = tokens.FirstOrDefault(t => t.Name == "access_token")?.Value;
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Unable to retrieve Strava Access Token for user {userId}", userId);
                throw new InvalidOperationException("User's Strava Access Token could not be obtained from the context.  Unable to query Strava for recent activities.");
            }

            var accessTokenExpiryDateString = tokens.FirstOrDefault(t => t.Name == "expires_at")?.Value;

            if (string.IsNullOrEmpty(accessTokenExpiryDateString) || !DateTimeOffset.TryParse(accessTokenExpiryDateString, out var accessTokenExpiryDateTimeOffset))
            {
                _logger.LogWarning("Unable to determine Strava Access Token expiration date for user {userId}", userId);
                return accessToken;
            }

            var utcNow = DateTime.UtcNow;
            var accessTokenExpiryUtc = accessTokenExpiryDateTimeOffset.ToUniversalTime();
            var isAccessTokenValid = utcNow < accessTokenExpiryUtc;

            if (isAccessTokenValid)
            {
                var timeRemaining = accessTokenExpiryUtc - utcNow;

                _logger.LogInformation(
                    "User's Strava Access Token expires at {accessTokenExpires:o}.  Current time is {utcNow:o}.  Access token is still valid.  Remaining lifetime: {timeRemaining}",
                    accessTokenExpiryUtc,
                    utcNow,
                    timeRemaining);

                return accessToken;
            }

            _logger.LogInformation(
                "User's Strava Access Token expired at {accessTokenExpires:o}.  Current time is {utcNow:o}.  Access token is not currently valid.",
                accessTokenExpiryUtc,
                utcNow);

            var refreshToken = tokens.FirstOrDefault(t => t.Name == "refresh_token")?.Value;
            if (string.IsNullOrEmpty(refreshToken))
            {
                _logger.LogWarning("Unable to retrieve Strava Refresh Token for user {userId}", userId);
                throw new InvalidOperationException("User's Strava Access Token could not be obtained from the context.  Unable to query Strava for recent activities.");
            }

            var httpClient = _httpClientFactory.CreateClient("Strava");

            var postBodyContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _stravaOptions.ClientId),
                new KeyValuePair<string, string>("client_secret", _stravaOptions.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
            });

            _logger.LogInformation("Attempting Strava token refresh ...");

            var response = await httpClient.PostAsync($"{ApiBaseUrl}/oauth/token", postBodyContent);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            var responseDataParsed = JsonConvert.DeserializeObject<StravaTokenRefreshResponse>(responseData);

            if (responseDataParsed == null)
            {
                _logger.LogWarning("Unable to parse response from Strava token refresh");
                throw new InvalidOperationException("User's Strava Access Token could not be obtained from the context.  Unable to query Strava for recent activities.");
            }

            _logger.LogInformation("Strava token refresh succeeded.  New access token expires at {newAccessTokenExpiry:o}", responseDataParsed.ExpiresAt.AsUtc());
            _logger.LogInformation("Persisting updated tokens ...");
            var user = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.SetAuthenticationTokenAsync(user, StravaAuthentication.AuthenticationScheme, "access_token", responseDataParsed.AccessToken);
            await _userManager.SetAuthenticationTokenAsync(user, StravaAuthentication.AuthenticationScheme, "refresh_token", responseDataParsed.RefreshToken);
            await _userManager.SetAuthenticationTokenAsync(user, StravaAuthentication.AuthenticationScheme, "token_type", responseDataParsed.TokenType);
            await _userManager.SetAuthenticationTokenAsync(user, StravaAuthentication.AuthenticationScheme, "expires_at", responseDataParsed.ExpiresAt.AsUtc().ToString("o"));

            return responseDataParsed.AccessToken;
        }
    }

    public class StravaTokenRefreshResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        private DateTime _expiresAt;

        [JsonProperty("expires_at")]
        [JsonConverter(typeof(JsonEpochConverter))]
        public DateTime ExpiresAt { get => _expiresAt; set => _expiresAt = value.AsUtc(); }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonProperty("token_type")]
        public string TokenType { get; set; } = string.Empty;
    }

    public class JsonEpochConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;

            var t = Convert.ToInt64(reader.Value);

            return t.IsTimestampInMilliseconds() ? GlobalConstants.UnixEpochUtc.AddMilliseconds(t) : GlobalConstants.UnixEpochUtc.AddSeconds(t);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var d = (DateTime)value;
            var t = (long)(d - GlobalConstants.UnixEpochUtc).TotalSeconds;
            writer.WriteValue(t);
        }
    }
}
