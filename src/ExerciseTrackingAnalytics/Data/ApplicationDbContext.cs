using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<UserActivity>? UserActivities { get; set; }

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
                .HasIndex(a => a.StravaActivityId)
                .IsUnique();

            modelBuilder.Entity<UserActivity>()
                .Property(a => a.RecordInsertDateUtc)
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql(GET_UTC_TIMESTAMP)
                .ValueGeneratedOnAdd();
        }

        private const string GET_UTC_TIMESTAMP = "now() AT TIME ZONE 'UTC'";
    }
}