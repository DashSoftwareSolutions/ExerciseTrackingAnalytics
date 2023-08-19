using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models.Identity;

namespace ExerciseTrackingAnalytics.Security.Authorization
{
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public ApplicationClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity?)principal.Identity;

            var claims = new List<Claim>();

            var hasFirstName = !string.IsNullOrWhiteSpace(user.FirstName);
            var hasLastName = !string.IsNullOrWhiteSpace(user.LastName);

            if (hasFirstName)
            {
                claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName!));
            }

            if (hasLastName)
            {
                claims.Add(new Claim(ClaimTypes.Surname, user.LastName!));
            }

            if (hasFirstName && hasLastName)
            {
                var fullName = $"{user.FirstName} {user.LastName}";
                claims.Add(new Claim(ClaimTypes.Name, fullName));
            }

            if (user.Tokens.HasAny())
            {
                foreach (var token in user.Tokens!)
                {
                    switch (token.Name)
                    {
                        case "access_token":
                        case "refresh_token":
                            claims.Add(new Claim($"{token.LoginProvider.ToLower()}_{token.Name}", token.Value));
                            break;

                        case "expires_at":
                            claims.Add(new Claim($"{token.LoginProvider.ToLower()}_access_token_{token.Name}", token.Value));
                            break;

                        default:
                            continue;
                    }
                }
            }

            identity?.AddClaims(claims);

            return principal;
        }
    }
}
