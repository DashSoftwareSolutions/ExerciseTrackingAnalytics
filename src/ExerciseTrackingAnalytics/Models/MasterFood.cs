using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExerciseTrackingAnalytics.Extensions;
using ExerciseTrackingAnalytics.Models.Identity;

namespace ExerciseTrackingAnalytics.Models
{
    public class MasterFood : NutritionalContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(1024)]
        public string Name { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string NameNormalized { get => _NameNormalized ?? Name.ToLowerInvariant(); private set => _NameNormalized = value; }
        private string? _NameNormalized;

        [Required]
        public int Version { get; set; } = 1;

        [MaxLength(1024)]
        public string? BrandName { get; set; }

        [Required]
        [Range(typeof(decimal), "1.00", "1000000.00", ErrorMessage = "{0} must be between {1} and {2}")]
        public decimal ServingSize { get; set; } = 1.00m;

        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string ServingSizeUnit { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "TIMESTAMP")]
        public DateTime RecordInsertDateUtc { get => _RecordInsertDateUtc; private set => _RecordInsertDateUtc = value.AsUtc(); }
        private DateTime _RecordInsertDateUtc;

        public Guid CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; private set; }

        public Guid? OwnerUserId { get; set; }
        public ApplicationUser? OwnerUser { get; private set; }

        [Required]
        public bool IsShared { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Column(TypeName = "TIMESTAMP")]
        public DateTime? DeactivatedDateUtc { get => _DeactivatedDateUtc; private set => _DeactivatedDateUtc = value.AsUtc(); }
        private DateTime? _DeactivatedDateUtc;

        public Guid? DeactivatedByUserId { get; set; }
        public ApplicationUser? DeactivatedByUser { get; private set; }

        public long? PredecessorId { get; set; }
        public MasterFood? Predecessor { get; private set; }
    }
}
