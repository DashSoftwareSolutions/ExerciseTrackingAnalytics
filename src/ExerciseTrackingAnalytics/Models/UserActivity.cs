using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models.Identity;

namespace ExerciseTrackingAnalytics.Models
{
    public class UserActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }

        [Required]
        public Guid UserId { get; set; }
        public ApplicationUser? User { get; private set; }

        [Required]
        public ExerciseTrackingApp ExternalApp { get; set; }
        
        [Required]
        public long ExternalAppActivityId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [MaxLength(32)]
        public string SportType { get; set; } = string.Empty;

        /// <summary>
        /// Activity Start Date UTC
        /// </summary>
        /// <remarks>
        /// <strong>NOTE:</strong> We <em>CANNOT</em> do our normal thing to set <c>Kind</c> to <c>Utc</c> here.<br />
        /// EF / Npgsql are apparently insisting on type <c>TIMESTAMPTZ</c> if <c>Kind</c> is <c>Utc</c> and we're deliberately using
        /// type <c>TIMESTAMP</c> (i.e. <c>TIMESTAMP WITHOUT TIME ZONE</c>).
        /// </remarks>
        [Required]
        [Column(TypeName = "TIMESTAMP")]
        public DateTime StartDateUtc { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(64)]
        public string TimeZone { get; set; } = string.Empty;

        [Required]
        public decimal DistanceInMeters { get; set; }

        [Required]
        public decimal DistanceInMiles { get; set; }

        /// <summary>
        /// Specifies which measure of the activity's distance was the original one (and which is a converted value)
        /// </summary>
        [Required]
        public DistanceUnit DistanceOriginalUnit { get; set; }

        public decimal? TotalElevationGainInMeters { get; set; }

        [Required]
        public int ElapsedTimeInSeconds { get; set; }

        [Required]
        public int MovingTimeInSeconds { get; set; }

        [Required]
        public decimal Calories { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "TIMESTAMP")]
        public DateTime RecordInsertDateUtc { get => _RecordInsertDateUtc; private set => _RecordInsertDateUtc = value.AsUtc(); }
        private DateTime _RecordInsertDateUtc;
    }
}
