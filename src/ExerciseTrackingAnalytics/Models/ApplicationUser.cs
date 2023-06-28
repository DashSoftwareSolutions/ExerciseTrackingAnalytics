using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ExerciseTrackingAnalytics.Models
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

        public virtual ICollection<IdentityUserClaim<Guid>>? Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<Guid>>? Logins { get; set; }

        public virtual ICollection<IdentityUserToken<Guid>>? Tokens { get; set; }

        public virtual ICollection<IdentityUserRole<Guid>>? UserRoles { get; set; }
    }
}
