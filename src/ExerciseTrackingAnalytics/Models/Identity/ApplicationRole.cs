using Microsoft.AspNetCore.Identity;

namespace ExerciseTrackingAnalytics.Models.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
}
