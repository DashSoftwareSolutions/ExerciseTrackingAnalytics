using NodaTime;

namespace ExerciseTrackingAnalytics.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// This method will produce a DateTime with .Kind set to DateTimeKind.Utc.  If the input DateTimeKind is Unspecified, then
        /// it will assume Utc and merely set .Kind.  If it is Local, then it will convert to the Utc timezone.  If Utc is already set
        /// then it will do nothing.
        /// </summary>
        public static DateTime AsUtc(this DateTime d)
        {
            switch (d.Kind)
            {
                case DateTimeKind.Local:
                    return TimeZoneInfo.ConvertTime(d, TimeZoneInfo.FindSystemTimeZoneById("UTC"));
                case DateTimeKind.Unspecified:
                    return DateTime.SpecifyKind(d, DateTimeKind.Utc);
                default:
                    return d;
            }
        }

        /// <summary>
        /// This method will produce a DateTime with .Kind set to DateTimeKind.Utc.  If the input DateTimeKind is Unspecified, then
        /// it will assume Utc and merely set .Kind.  If it is Local, then it will convert to the Utc timezone.  If Utc is already set
        /// or if d is null, then it will do nothing.
        /// </summary>
        public static DateTime? AsUtc(this DateTime? d)
        {
            return d.HasValue ? d.Value.AsUtc() : d;
        }

        /// <summary>
        /// This method will produce a DateTime object with d.Kind set to DateTimeKind.Unspecified from a long that represents a Unix timestamp.
        /// </summary>
        public static DateTime ToDateTime(this long l)
        {
            DateTime d = Constants.UnixEpochUtc;
            return d.AddSeconds(l);
        }

        /// <summary>
        /// Converts a <see cref="TimeSpan"/> to [+/-]HH:mm format
        /// </summary>
        /// <remarks>
        /// * Always includes sign (+ or -)
        /// * Always includes leading 0 for single digit hours
        /// </remarks>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public static string ToHMString(this TimeSpan timespan)
        {
            var isNegative = timespan.Ticks < 0;
            var sign = isNegative ? "-" : "+";
            var actualTimeSpan = isNegative ? timespan.Negate() : timespan;
            return $"{sign}{actualTimeSpan.Hours.ToString("00")}:{actualTimeSpan.Minutes.ToString("00")}";
        }

        /// <summary>
        /// This method will not do a conversion for you, and d.Kind must be DateTimeKind.Utc or it will throw ArgumentException.
        /// </summary>
        public static long ToUnixTimestamp(this DateTime d, bool milliseconds = false)
        {
            if (d.Kind != DateTimeKind.Utc)
                throw new ArgumentException("We don't serve your kind here!");

            var elapsedSinceEpoch = d - Constants.UnixEpochUtc;
            return (long)(milliseconds ? elapsedSinceEpoch.TotalMilliseconds : elapsedSinceEpoch.TotalSeconds);
        }

        /// <summary>
        /// This method will not do a conversion for you, and d.Kind must be DateTimeKind.Utc or it will throw ArgumentException.
        /// </summary>
        public static long? ToUnixTimestamp(this DateTime? d, bool milliseconds = false)
        {
            if (d.HasValue) return d.Value.ToUnixTimestamp(milliseconds);

            return null;
        }

        public static DateTime WithTimeZone(this DateTime utcDateTime, string tzdbTimeZoneId)
        {
            if (utcDateTime.Kind != DateTimeKind.Utc)
                throw new InvalidOperationException("This operation can only be performed on UTC Date/Times");

            var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(tzdbTimeZoneId);

            if (timeZone == null)
                throw new ArgumentException(
                    $"'{tzdbTimeZoneId}' is not a valid IANA Time Zone Database ID",
                    nameof(tzdbTimeZoneId));

            return LocalDateTime
                .FromDateTime(utcDateTime)
                .InZoneStrictly(DateTimeZone.Utc)
                .WithZone(timeZone)
                .ToDateTimeUnspecified();
        }

        public static bool IsValidTimeZoneId(this string possibleTimeZoneId)
        {
            return DateTimeZoneProviders.Tzdb.Ids.Contains(possibleTimeZoneId);
        }

        /// <summary>
        /// Convert UTC time to specified time zone
        /// </summary>
        /// <param name="utcDateTime">Date-time in UTC</param>
        /// <param name="timeZone">IANA Time zone ID to convert to</param>
        /// <param name="throwException">If true, the method throws exception if no time zone found</param>
        /// <returns>Date time in the specified time zone</returns>
        public static DateTime ConvertToTimeZoneFromUtc(this DateTime utcDateTime, string timeZone, bool throwException = false)
        {
            var destTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZone ?? string.Empty);

            if (destTimeZone == null)
            {
                // Unsupported time zone
                if (throwException)
                    throw new ArgumentException($"'{timeZone ?? "NULL"}' is not a valid IANA Time Zone Database ID (e.g. America/Los_Angeles for U.S. Pacific Time)", nameof(timeZone));

                // Just fallback on machine's time zone in this case
                return utcDateTime.ToLocalTime();
            }

            return Instant.FromDateTimeUtc(utcDateTime.AsUtc())
                          .InZone(destTimeZone)
                          .ToDateTimeUnspecified();
        }

        /// <summary>
        /// Convert time in specified time zone to UTC 
        /// </summary>
        /// <param name="dateTime">Date-time in specified time zone</param>
        /// <param name="timeZone">Time zone to convert from</param>
        /// <param name="throwException">If true, the method throws exception if no time zone found</param>
        /// <returns>Date time in UTC</returns>
        public static DateTime ConvertToUtcFromTimeZone(this DateTime dateTime, string timeZone, bool throwException = false)
        {
            var sourceTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZone);

            if (sourceTimeZone == null)
            {
                // Unsupported time zone
                if (throwException)
                    throw new ArgumentException($"'{timeZone ?? "NULL"}' is not a valid IANA Time Zone Database ID (e.g. America/Los_Angeles for U.S. Pacific Time)", nameof(timeZone));

                // Return UTC from local time in this case
                return dateTime.ToUniversalTime();
            }

            LocalDateTime local = LocalDateTime.FromDateTime(dateTime);
            ZonedDateTime zonedTime = local.InZoneLeniently(sourceTimeZone);
            return zonedTime.ToDateTimeUtc();
        }
    }
}
