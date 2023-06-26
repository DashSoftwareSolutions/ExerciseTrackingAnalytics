using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace ExerciseTrackingAnalytics.Security.Authorization.Strava
{
    public static class Constants
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "Strava";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public static readonly string DisplayName = "Strava";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public static readonly string Issuer = "Strava";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public static readonly string CallbackPath = "/Connect/Callback";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://www.strava.com/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public static readonly string TokenEndpoint = "https://www.strava.com/oauth/token";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://www.strava.com/api/v3/athlete";

        /// <summary>
        /// Strava OAuth Scope enabling basic read access.<br />
        /// "Read public segments, public routes, public profile data, public posts, public events, club feeds, and leaderboards."<br />
        /// <see href="https://developers.strava.com/docs/authentication/#details-about-requesting-access"/>
        /// </summary>
        public static readonly string BasicReadAccessScope = "read";

        /// <summary>
        /// Strava OAuth Scope enabling read of all Activity resource data.<br />
        /// "Read the user's activity data for activities that are visible to Everyone and Followers...plus privacy zone data and access to read the user's activities with visibility set to Only You."<br />
        /// <see href="https://developers.strava.com/docs/authentication/#details-about-requesting-access"/>
        /// </summary>
        public static readonly string ReadAllActivitiesScope = "activity:read_all";

        /// <summary>
        /// Additional Strava Profile Claims
        /// </summary>
        public static class StravaCustomClaimTypes
        {
            public const string City = "urn:strava:city";
            public const string CreatedAt = "urn:strava:created-at";
            public const string Premium = "urn:strava:premium";
            public const string Profile = "urn:strava:profile";
            public const string ProfileMedium = "urn:strava:profile-medium";
            public const string UpdatedAt = "urn:strava:updated-at";
        }
    }
}
