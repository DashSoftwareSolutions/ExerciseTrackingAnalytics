using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExerciseTrackingAnalytics.Extensions;

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
        public long StravaActivityId { get; set; }

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
        /// <strong>NOTE:</strong> We <em>CANNOT</em> do our normal thing to set <c>Kind</c> to <c>Utc</c> here.
        /// EF / Npgsql are apparently insisting on type `TIMESTAMPTZ` if `Kind` is `Utc` and we're deliberately using
        /// type `TIMESTAMP` (i.e. `TIMESTAMP WITHOUT TIME ZONE`).
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
        public decimal TotalElevationGainInMeters { get; set; }

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
