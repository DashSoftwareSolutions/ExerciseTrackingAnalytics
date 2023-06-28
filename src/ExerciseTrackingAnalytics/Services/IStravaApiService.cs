using ExerciseTrackingAnalytics.Models;

namespace ExerciseTrackingAnalytics.Services
{
    public interface IStravaApiService
    {
        /// <summary>
        /// Fetches a collection a recent Activities for the context user
        /// </summary>
        /// <remarks>
        /// <see href="https://developers.strava.com/docs/reference/#api-Activities-getLoggedInAthleteActivities"/>
        /// </remarks>
        /// <param name="beforeDateUtc">Optional UTC date/time to use for filtering activities that have taken place before a certain time.</param>
        /// <param name="afterDateUtc">Optional UTC date/time to use for filtering activities that have taken place after a certain time.</param>
        /// <param name="pageNumber">Page number. Defaults to 1.</param>
        /// <param name="pageSize">Number of items per page. Defaults to 30.</param>
        /// <returns>Collection of Activities</returns>
        Task<IEnumerable<StravaActivity>> GetRecentActivitiesAsync(
            DateTime? beforeDateUtc = null,
            DateTime? afterDateUtc = null,
            int pageNumber = 1,
            int pageSize = 30);

        /// <summary>
        /// Fetches the details for the specified <paramref name="activity"/> and hydrates the <b><c>Calories</c></b> (which is not included in the summary data)
        /// </summary>
        /// <remarks>
        /// Calls API endpoint <see href="https://developers.strava.com/docs/reference/#api-Activities-getActivityById"/>
        /// </remarks>
        /// <param name="activity">Activity to operate on</param>
        /// <returns></returns>
        Task HydrateActivityCaloriesAsync(StravaActivity activity);
    }
}
