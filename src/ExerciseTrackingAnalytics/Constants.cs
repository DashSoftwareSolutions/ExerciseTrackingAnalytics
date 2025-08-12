namespace ExerciseTrackingAnalytics
{
    public static class Constants
    {
        public const string DataProtectionPurpose = "ExerciseTrackingAnalytics.SensitiveUserTokenProtection";

        public const decimal MilesPerMeter = 0.0006213712m;

        public const decimal FeetPerMeter = 3.280839895m;

        /// <summary>
        /// System-wide default Time Zone
        /// </summary>
        public const string DefaultTimeZoneId = "America/Los_Angeles";

        public static readonly DateTime UnixEpochUtc = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    }
}
