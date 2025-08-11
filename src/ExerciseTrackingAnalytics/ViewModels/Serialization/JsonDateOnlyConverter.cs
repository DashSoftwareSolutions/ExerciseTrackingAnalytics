using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExerciseTrackingAnalytics.ViewModels.Serialization
{
    /// <summary>
    /// Custom JSON Converter for <see cref="DateOnly"/>s that serializes the values as <c>YYYY-MM-DD</c> strings.
    /// </summary>
    public class JsonDateOnlyConverter : JsonConverter<DateOnly>
    {
        private static readonly CultureInfo _enUS = new CultureInfo("en-US");

        public override DateOnly Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var value = reader.GetString();

            if (!string.IsNullOrWhiteSpace(value) &&
                DateTime.TryParseExact(value, "yyyy-MM-dd", _enUS, DateTimeStyles.None, out var date))
            {
                return DateOnly.FromDateTime(date);
            }

            throw new JsonException("Invalid date format");
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateOnly dateTimeValue,
            JsonSerializerOptions options)
            => writer.WriteStringValue(dateTimeValue.ToString("yyyy-MM-dd", _enUS));
    }
}
