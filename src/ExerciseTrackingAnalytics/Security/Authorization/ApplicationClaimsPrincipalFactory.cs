using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using IdentityModel;
using ExerciseTrackingAnalytics.Models;

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

#pragma warning disable CS8604 // Possible null reference argument.
            if (hasFirstName)
            {
                claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
                claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
            }

            if (hasLastName)
            {
                claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
                claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
            }
#pragma warning restore CS8604 // Possible null reference argument.

            if (hasFirstName && hasLastName)
            {
                var fullName = $"{user.FirstName} {user.LastName}";
                claims.Add(new Claim(JwtClaimTypes.Name, fullName));
                claims.Add(new Claim(ClaimTypes.Name, fullName));
            }

            identity?.AddClaims(claims);

            return principal;
        }
    }
}
