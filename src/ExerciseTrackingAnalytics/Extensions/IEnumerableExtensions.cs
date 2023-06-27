namespace ExerciseTrackingAnalytics.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Safe alternative to IEnumerable&lt;T&gt;.Any() method that includes a null check, providing some syntactic sugar.
        /// </summary>
        /// <typeparam name="T">the type of thing in the collection</typeparam>
        /// <param name="list">IEnumerable&lt;T&gt;</param>
        /// <returns>Boolean indicating the collection is not null and has at least one item in it</returns>
        public static bool HasAny<T>(this IEnumerable<T> list) => list != null && list.Any();

        /// <summary>
        /// Safe alternative to IEnumerable&lt;T&gt;.Any() method that includes a null check, providing some syntactic sugar.
        /// </summary>
        /// <typeparam name="T">the type of thing in the collection</typeparam>
        /// <param name="list">IEnumerable&lt;T&gt;</param>
        /// <param name="predicate">Func&lt;T, bool&lt;</param>
        /// <returns>Boolean indicating the collection is not null and has at least one item in it that matches the predicate</returns>
        public static bool HasAny<T>(this IEnumerable<T> list, Func<T, bool> predicate) => list != null && list.Any(predicate);
    }
}
