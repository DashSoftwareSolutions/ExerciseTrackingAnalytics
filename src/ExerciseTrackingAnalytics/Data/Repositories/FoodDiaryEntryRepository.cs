using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using ExerciseTrackingAnalytics.Exceptions;
using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Data.Repositories
{
    public class FoodDiaryEntryRepository : IFoodDiaryEntryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<FoodDiaryEntryRepository> _logger;

        public FoodDiaryEntryRepository(ApplicationDbContext db, ILogger<FoodDiaryEntryRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public Task<FoodDiaryEntry?> GetByIdAsync(long foodDiaryEntryId)
        {
            return _db
                .FoodDiaryEntries!
                .FirstOrDefaultAsync(fde => fde.Id == foodDiaryEntryId);
        }

        public async Task<IEnumerable<FoodDiaryEntry>> GetByUserAndDateAsync(Guid userId, DateOnly date)
        {
            var results = await _db
                .FoodDiaryEntries!
                .Where(fde => fde.OwnerUserId == userId && fde.Date == date)
                .OrderBy(fde => fde.TimeOfDay)
                .ToArrayAsync();
            
            return results.ToImmutableArray().AsEnumerable();
        }

        public async Task<IEnumerable<FoodDiaryEntry>> GetByUserAndDateRangeAsync(Guid userId, DateOnly startDate, DateOnly endDate)
        {
            var results = await _db
                .FoodDiaryEntries!
                .Where(fde => fde.OwnerUserId == userId && fde.Date >= startDate && fde.Date <= endDate)
                .OrderBy(fde => fde.TimeOfDay)
                .ToArrayAsync();

            return results.ToImmutableArray().AsEnumerable();
        }

        public async Task<FoodDiaryEntry> InsertAsync(FoodDiaryEntry foodDiaryEntry)
        {
            await _db.FoodDiaryEntries!.AddAsync(foodDiaryEntry);
            await _db.SaveChangesAsync();
            var result = await GetByIdAsync(foodDiaryEntry.Id);

            return result
                ?? throw new ExerciseTrackingAnalyticsAppException("Unable to save food diary entry");
        }
    }
}
