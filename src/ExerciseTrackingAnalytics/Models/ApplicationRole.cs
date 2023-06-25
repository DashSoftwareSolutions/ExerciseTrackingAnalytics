using Microsoft.AspNetCore.Identity;

namespace ExerciseTrackingAnalytics.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
}
