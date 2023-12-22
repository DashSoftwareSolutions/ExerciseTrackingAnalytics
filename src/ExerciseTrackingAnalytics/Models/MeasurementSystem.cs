namespace ExerciseTrackingAnalytics.Models
{
    public enum MeasurementSystem
    {
        Unspecified = 0,

        /// <summary>
        /// Customary units of measure in use in the United States, e.g. inches, feet, yards, miles, ounces, pounds, etc.<br />
        /// <see href="https://en.wikipedia.org/wiki/United_States_customary_units"/>
        /// </summary>
        UnitedStates = 1,

        /// <summary>
        /// "The <b>International System of Units</b>, internationally known by the abbreviation <b>SI</b> (from French <em>Système International</em>), is the modern form of the <see href="https://en.wikipedia.org/wiki/Metric_system">metric system</see> and the world's most widely used system of measurement."<br />
        /// Meters, kilometers, grams, kilograms, etc.<br />
        /// <see href="https://en.wikipedia.org/wiki/International_System_of_Units"/>
        /// </summary>
        MetricSystem = 2,
    }
}
