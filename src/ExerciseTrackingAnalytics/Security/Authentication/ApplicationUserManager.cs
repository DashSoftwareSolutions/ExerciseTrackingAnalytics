using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Dapper;
using Npgsql;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Data;
using ExerciseTrackingAnalytics.Models.Identity;
using static ExerciseTrackingAnalytics.Constants;

namespace ExerciseTrackingAnalytics.Security.Authentication
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private static readonly string[] _ProtectedTokenNames = new[] { "access_token", "refresh_token" };

        private readonly IDataProtector _dataProtector;
        private readonly ApplicationDbContext _dbContext;

        public ApplicationUserManager(
            ApplicationDbContext dbContext,
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
            _dbContext = dbContext;

            var dataProtectionProvider = services.GetRequiredService<IDataProtectionProvider>();
            _dataProtector = dataProtectionProvider.CreateProtector(DataProtectionPurpose);
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
            // Don't know of it's ASP.NET Identity's fault or Entity Frameworks's fault, but one of those suckas is to blame.
            // It seems to be a combination: like my customized ASP.NET Identity / EF setup doesn't seem to really know about User Tokens
            if (!user.Tokens.HasAny())
            {
                var tokens = await GetTokens(user.Id, loginProvider);

                if (tokens.Any())
                {
                    user.Tokens = tokens.ToArray();
                }
            }

            return user;
        }

        public override async Task<string> GetAuthenticationTokenAsync(ApplicationUser user, string loginProvider, string tokenName)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (loginProvider == null)
            {
                throw new ArgumentNullException(nameof(loginProvider));
            }

            if (tokenName == null)
            {
                throw new ArgumentNullException(nameof(tokenName));
            }

            using (var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString()))
            {
                var userToken = await connection.QueryFirstOrDefaultAsync<ApplicationUserToken>(@"
          SELECT ""UserId""
                ,""LoginProvider""
                ,""Name""
                ,""Value""
            FROM ""AspNetUserTokens""
           WHERE ""UserId"" = @userId
             AND ""LoginProvider"" = @loginProvider
             AND ""Name"" = @tokenName;
        ",
                    new { userId = user.Id, loginProvider, tokenName });

                if (userToken?.Value == null)
                {
                    return string.Empty;
                }

                return _ProtectedTokenNames.Contains(userToken.Name)
                    ? UnprotectPersonalData(userToken.Value)
                    : userToken.Value;
            }
        }

        public override async Task<IdentityResult> SetAuthenticationTokenAsync(ApplicationUser user, string loginProvider, string tokenName, string tokenValue)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (loginProvider == null)
            {
                throw new ArgumentNullException(nameof(loginProvider));
            }

            if (tokenName == null)
            {
                throw new ArgumentNullException(nameof(tokenName));
            }

            var isProtectedToken = _ProtectedTokenNames.Contains(tokenName);

            Logger.LogInformation(
                "Setting authentication token for User ID {userId} and Login Provider {loginProvider}: {tokenName} = {tokenValue}",
                user.Id,
                loginProvider,
                tokenName,
                isProtectedToken ? tokenValue.MaskSecretValue() : tokenValue);

            try
            {
                using (var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString()))
                {
                    var tokenValueToPersist = isProtectedToken
                        ? ProtectPersonalData(tokenValue)
                        : tokenValue;

                    var rowsAffected = await connection.ExecuteAsync(@"
        INSERT INTO ""AspNetUserTokens"" ( ""UserId"", ""LoginProvider"", ""Name"", ""Value"" )
        VALUES ( @userId, @loginProvider, @tokenName, @tokenValue )
        ON CONFLICT ( ""UserId"", ""LoginProvider"", ""Name"" )
        DO UPDATE SET ""Value"" = @tokenValue;
        ",
                        new { userId = user.Id, loginProvider, tokenName, tokenValue = tokenValueToPersist });

                    return rowsAffected == 1
                        ? IdentityResult.Success
                        : IdentityResult.Failed();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(
                    ex,
                    "Error setting authentication token {tokenName} for User ID {userId} and Login Provider {loginProvider}",
                    tokenName,
                    user.Id,
                    loginProvider);

                return IdentityResult.Failed();
            }
        }

        private async Task<IEnumerable<ApplicationUserToken>> GetTokens(Guid userId, string loginProvider)
        {
            using (var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString()))
            {
                var userTokens = await connection.QueryAsync<ApplicationUserToken>(@"
          SELECT ""UserId""
                ,""LoginProvider""
                ,""Name""
                ,""Value""
            FROM ""AspNetUserTokens""
           WHERE ""UserId"" = @userId
             AND ""LoginProvider"" = @loginProvider
        ORDER BY ""Name"";
        ",
                    new { userId, loginProvider });

                if (userTokens.HasAny())
                {
                    // Unprotect (decrypt) the token values if necessary
                    userTokens = userTokens.ToArray(); // materialize the collection so the objects can be mutated
                    UnprotectUserTokens(userTokens);
                }

                return userTokens;
            }
        }

        public override async Task<IdentityResult> RemoveAuthenticationTokenAsync(ApplicationUser user, string loginProvider, string tokenName)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (loginProvider == null)
            {
                throw new ArgumentNullException(nameof(loginProvider));
            }

            if (tokenName == null)
            {
                throw new ArgumentNullException(nameof(tokenName));
            }

            Logger.LogInformation(
                "Removing authentication token {tokenName} for User ID {userId} and Login Provider {loginProvider}",
                tokenName,
                user.Id,
                loginProvider);

            try
            {
                using (var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString()))
                {
                    var rowsAffected = await connection.ExecuteAsync(@"
         DELETE FROM ""AspNetUserTokens""
               WHERE ""UserId"" = @userId
                 AND ""LoginProvider"" = @loginProvider
                 AND ""Name"" = @tokenName;
        ",
                        new { userId = user.Id, loginProvider, tokenName });

                    return rowsAffected == 1
                        ? IdentityResult.Success
                        : IdentityResult.Failed();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(
                    ex,
                    "Error removing authentication token {tokenName} for User ID {userId} and Login Provider {loginProvider}",
                    tokenName,
                    user.Id,
                    loginProvider);

                return IdentityResult.Failed();
            }
        }

        private string ProtectPersonalData(string data)
        {
            if (Options.Stores.ProtectPersonalData)
            {
                return _dataProtector.Protect(data);
            }

            return data;
        }

        private void ProtectUserTokens(IEnumerable<ApplicationUserToken> userTokens)
        {
            if (Options.Stores.ProtectPersonalData)
            {
                foreach (var userToken in userTokens)
                {
                    if (_ProtectedTokenNames.Contains(userToken.Name))
                    {
                        userToken.Value = _dataProtector.Protect(userToken.Value);
                    }
                }
            }
        }

        private string UnprotectPersonalData(string protectedData)
        {
            if (Options.Stores.ProtectPersonalData)
            {
                return _dataProtector.Unprotect(protectedData);
            }

            return protectedData;
        }

        private void UnprotectUserTokens(IEnumerable<ApplicationUserToken> userTokens)
        {
            if (Options.Stores.ProtectPersonalData)
            {
                foreach (var userToken in userTokens)
                {
                    
                    if (_ProtectedTokenNames.Contains(userToken.Name))
                    {
                        userToken.Value = _dataProtector.Unprotect(userToken.Value);
                    }
                }
            }
        }
    }
}
