using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models;
using System.Text.Json;

namespace ExerciseTrackingAnalytics.Security.Authentication
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(
            IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public string? GetUserFirstName(ClaimsPrincipal user)
        {
            return user?.GetUserFirstName();
        }

        public string? GetUserFullName(ClaimsPrincipal user)
        {
            return user?.GetUserFullName();
        }

        public string? GetUserLastName(ClaimsPrincipal user)
        {
            return user?.GetUserLastName();
        }

        public override async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            var result = await base.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                var retrievedUser = await Store.FindByIdAsync(user.Id.ToString(), CancellationToken);

                if (retrievedUser != null)
                {
                    retrievedUser.EmailConfirmedDate = DateTime.UtcNow;
                    await Store.UpdateAsync(retrievedUser, CancellationToken);
                }
            }

            return result;
        }

        public override async Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey)
        {
            Logger.LogInformation(
                "Querying for User with external Key {providerKey} from Login Provider {loginProvider}",
                providerKey,
                loginProvider);

            var user = await base.FindByLoginAsync(loginProvider, providerKey);

            if (user == null)
            {
                Logger.LogInformation("User not found");

#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }

            Logger.LogInformation("Found User ID {userId} {username}", user.Id, user.UserName);

            // This kind of sucks.
            // For some reason, it is not expanding `user.Tokens` when the User is fetched.
            // So, we have to do it ourselves here.
            // And, even worse, we cannot get a collection of them all at once; we can only get them one by one.  Sigh.
            // Don't know of it's ASP.NET Identity's fault or Entity Frameworks's fault, but one of those suckas is to blame.
            if (!user.Tokens.HasAny())
            {
                var tokens = await GetTokens(user, loginProvider);

                if (tokens.Any())
                {
                    user.Tokens = tokens;
                }
            }

            return user;
        }

        private async Task<List<IdentityUserToken<Guid>>> GetTokens(ApplicationUser user, string loginProvider)
        {
            var tokens = new List<IdentityUserToken<Guid>>();

            var userAuthnTokenStore = (Store as IUserAuthenticationTokenStore<ApplicationUser>);

            if (userAuthnTokenStore == null)
            {
                Logger.LogWarning("Store backing User Manager does not support retrieval of User Authentication Tokens.");
                return tokens;
            }

            await TryGetAndAddToken(tokens, userAuthnTokenStore!, user, loginProvider, "access_token");
            await TryGetAndAddToken(tokens, userAuthnTokenStore!, user, loginProvider, "expires_at");
            await TryGetAndAddToken(tokens, userAuthnTokenStore!, user, loginProvider, "refresh_token");

            return tokens;
        }

        private async Task<bool> TryGetAndAddToken(
            List<IdentityUserToken<Guid>> tokens,
            IUserAuthenticationTokenStore<ApplicationUser> userAuthnTokenStore,
            ApplicationUser user,
            string loginProvider,
            string tokenName)
        {
            var token = await userAuthnTokenStore!.GetTokenAsync(user, loginProvider, tokenName, CancellationToken);

            if (token != null)
            {
                tokens.Add(new IdentityUserToken<Guid>() { LoginProvider = loginProvider, Name = tokenName, Value = token });
                return true;
            }

            return false;
        }
    }
}
