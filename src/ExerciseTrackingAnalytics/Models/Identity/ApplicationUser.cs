using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ExerciseTrackingAnalytics.Models.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        [MaxLength(70)]
        public string? FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        [MaxLength(70)]
        public string? LastName { get; set; }

        public DateTime? EmailConfirmedDate { get; set; }

        public DateTime? PhoneConfirmedDate { get; set; }

        public virtual ICollection<ApplicationUserClaim>? Claims { get; set; }

        public virtual ICollection<ApplicationUserLogin>? Logins { get; set; }

        public virtual ICollection<ApplicationUserToken>? Tokens { get; set; }

        public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
    }
}
