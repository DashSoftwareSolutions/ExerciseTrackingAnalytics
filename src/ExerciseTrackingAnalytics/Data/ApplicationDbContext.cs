using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExerciseTrackingAnalytics.Models;
using ExerciseTrackingAnalytics.Models.Identity;

namespace ExerciseTrackingAnalytics.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<
            ApplicationUser,
            ApplicationRole,
            Guid,
            ApplicationUserClaim,
            ApplicationUserRole,
            ApplicationUserLogin,
            ApplicationRoleClaim,
            ApplicationUserToken>
    {
        public DbSet<UserActivity>? UserActivities { get; set; }

        public DbSet<MasterFood>? MasterFoods { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Identity Stuff
            modelBuilder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            // Activities
            modelBuilder.Entity<UserActivity>()
                .Property(e => e.ExternalApp)
                .HasColumnType("VARCHAR(128)")
                .HasConversion(
                    v => v.ToString(),
                    v => (ExerciseTrackingApp)Enum.Parse(typeof(ExerciseTrackingApp), v));

            modelBuilder.Entity<UserActivity>()
                .Property(e => e.DistanceOriginalUnit)
                .HasColumnType("VARCHAR(32)")
                .HasConversion(
                    v => v.ToString(),
                    v => (DistanceUnit)Enum.Parse(typeof(DistanceUnit), v));


            modelBuilder.Entity<UserActivity>()
                .HasIndex(a => new { a.ExternalApp, a.ExternalAppActivityId })
                .IsUnique();

            modelBuilder.Entity<UserActivity>()
                .Property(a => a.RecordInsertDateUtc)
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql(GET_UTC_TIMESTAMP)
                .ValueGeneratedOnAdd();

            // Master Food
            modelBuilder.Entity<MasterFood>()
                .Property(mf => mf.RecordInsertDateUtc)
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql(GET_UTC_TIMESTAMP)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MasterFood>()
                .HasIndex(mf => new { mf.NameNormalized, mf.Version })
                .IsUnique();

            // TODO: Food Diary Entry
        }

        private const string GET_UTC_TIMESTAMP = "now() AT TIME ZONE 'UTC'";
    }
}