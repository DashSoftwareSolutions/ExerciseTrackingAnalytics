using ExerciseTrackingAnalytics.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTrackingAnalytics.Data.Repositories
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UserActivityRepository> _logger;

        public UserActivityRepository(ApplicationDbContext db, ILogger<UserActivityRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public Task<bool> ExistsByExternalAppIdAsync(ExerciseTrackingApp externalApp, long externalAppActivityId)
        {
            return _db.UserActivities!.AnyAsync(a =>
                a.ExternalApp == externalApp &&
                a.ExternalAppActivityId == externalAppActivityId);
        }

        public Task<UserActivity?> GetByIdAsync(long id)
        {
            return _db.UserActivities!.FirstOrDefaultAsync(a => a.Id == id);
        }

        public Task<UserActivity?> GetByExternalAppIdAsync(ExerciseTrackingApp externalApp, long externalAppActivityId)
        {
            return _db.UserActivities!.FirstOrDefaultAsync(a =>
                a.ExternalApp == externalApp &&
                a.ExternalAppActivityId == externalAppActivityId);
        }

        public async Task<UserActivity?> InsertAsync(UserActivity userActivity)
        {
            await _db.UserActivities!.AddAsync(userActivity);
            await _db.SaveChangesAsync();
            return await GetByIdAsync(userActivity.Id);
        }
    }
}
