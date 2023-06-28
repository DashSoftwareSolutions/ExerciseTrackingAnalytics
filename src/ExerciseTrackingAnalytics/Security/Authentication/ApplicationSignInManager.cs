using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ExerciseTrackingAnalytics.Models;
using ExerciseTrackingAnalytics.Extensions;

namespace ExerciseTrackingAnalytics.Security.Authentication
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser>
    {
        public ApplicationSignInManager(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<ApplicationUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<ApplicationUser> confirmation) :
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public override Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            Logger.LogInformation("Overridden ExternalLoginSignInAsync() called");

            return base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
        }

        public override async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            Logger.LogInformation("Overridden ExternalLoginSignInAsync() called");

            //return base.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);

            var user = await UserManager.FindByLoginAsync(loginProvider, providerKey);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            var error = await PreSignInCheck(user);
            
            if (error != null)
            {
                return error;
            }

            return await SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);
        }

        public override Task<IdentityResult> UpdateExternalAuthenticationTokensAsync(ExternalLoginInfo externalLogin)
        {
            Logger.LogInformation("Overridden UpdateExternalAuthenticationTokensAsync() called");

            // TODO/FIXME: Don't be logging tokens!  Just temporary for debuggin'
            Logger.LogInformation("externalLogin.AuthenticationTokens: {authenticationTokens}", JsonSerializer.Serialize(externalLogin.AuthenticationTokens));

            return base.UpdateExternalAuthenticationTokensAsync(externalLogin);
        }
    }
}
