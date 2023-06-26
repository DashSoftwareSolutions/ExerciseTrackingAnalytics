using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static ExerciseTrackingAnalytics.Security.Authorization.Strava.Constants;

namespace ExerciseTrackingAnalytics.Security.Authorization.Strava
{
    public class StravaOAuthOptions : OAuthOptions
    {
        public StravaOAuthOptions()
        {
            ClaimsIssuer = Issuer;

            CallbackPath = Constants.CallbackPath;

            AuthorizationEndpoint = Constants.AuthorizationEndpoint;
            TokenEndpoint = Constants.TokenEndpoint;
            UserInformationEndpoint = Constants.UserInformationEndpoint;

            Scope.Add(BasicReadAccessScope);
            Scope.Add(ReadAllActivitiesScope);

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstname");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastname");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.StateOrProvince, "state");
            ClaimActions.MapJsonKey(ClaimTypes.Country, "country");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "sex");
            ClaimActions.MapJsonKey(StravaCustomClaimTypes.City, "city");
            ClaimActions.MapJsonKey(StravaCustomClaimTypes.Profile, "profile");
            ClaimActions.MapJsonKey(StravaCustomClaimTypes.ProfileMedium, "profile_medium");
            ClaimActions.MapJsonKey(StravaCustomClaimTypes.CreatedAt, "created_at");
            ClaimActions.MapJsonKey(StravaCustomClaimTypes.UpdatedAt, "updated_at");
            ClaimActions.MapJsonKey(StravaCustomClaimTypes.Premium, "premium");
        }
    }
}
