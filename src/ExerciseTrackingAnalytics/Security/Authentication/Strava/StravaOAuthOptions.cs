/*
 * Adapted from: https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/src/AspNet.Security.OAuth.Strava/StravaAuthenticationOptions.cs
 */
/*
* Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
* See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
* for more information concerning the license and the contributors participating to this project.
*/
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static ExerciseTrackingAnalytics.Security.Authentication.Strava.Constants;

namespace ExerciseTrackingAnalytics.Security.Authentication.Strava
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

            SaveTokens = true;

            Events.OnCreatingTicket = async (oAuthContext) =>
            {
                var tokens = oAuthContext.Properties.GetTokens().ToArray();
                oAuthContext.Properties.StoreTokens(tokens);

                if (tokens.Length > 0)
                {
                    // We need to store the tokens here, then get them out of the AuthenticationProperties
                    var userManager = oAuthContext.HttpContext.RequestServices.GetRequiredService<ApplicationUserManager>();
                    var user = await userManager.FindByLoginAsync(Constants.AuthenticationScheme, oAuthContext.User.GetProperty("id").ToString());

                    foreach (var token in tokens)
                    {
                        await userManager.SetAuthenticationTokenAsync(user, Constants.AuthenticationScheme, token.Name, token.Value);
                        oAuthContext.Properties.Items.Remove($".Token.{token.Name}"); // remove the tokens from the authentication properties
                    }
                }
            };
        }
    }
}
