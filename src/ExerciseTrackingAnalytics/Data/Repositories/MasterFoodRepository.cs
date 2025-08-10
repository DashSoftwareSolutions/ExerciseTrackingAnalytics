using Microsoft.EntityFrameworkCore;
using ExerciseTrackingAnalytics.Exceptions;
using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Data.Repositories
{
    public class MasterFoodRepository : IMasterFoodRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<MasterFoodRepository> _logger;

        public MasterFoodRepository(ApplicationDbContext db, ILogger<MasterFoodRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public Task<MasterFood?> GetByIdAsync(long masterFoodId)
        {
            return _db
                .MasterFoods!
                .FirstOrDefaultAsync(mf => mf.Id == masterFoodId);
        }

        public async Task<MasterFood> InsertAsync(MasterFood masterFood)
        {
            await _db.MasterFoods!.AddAsync(masterFood);
            await _db.SaveChangesAsync();
            var result = await GetByIdAsync(masterFood.Id);
            
            return result
                ?? throw new ExerciseTrackingAnalyticsAppException(
                    $"Persistence of Master food {masterFood.Name} version {masterFood.Version} failed.  Could not retrieve newly inserted record.");
        }
    }
}
