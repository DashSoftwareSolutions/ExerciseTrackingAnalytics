using System.Text.Json.Serialization;

namespace ExerciseTrackingAnalytics.ViewModels
{
    public class StravaWebhookEventViewModel
    {
        [JsonPropertyName("aspect_type")]
        public string AspectType { get; set; } = string.Empty;

        [JsonPropertyName("event_time")]
        public long EventTime { get; set; }

        [JsonPropertyName("object_id")]
        public long ObjectId { get; set; }

        [JsonPropertyName("object_type")]
        public string ObjectType { get; set; } = string.Empty;

        [JsonPropertyName("owner_id")]
        public long OwnerId { get; set; }

        [JsonPropertyName("subscription_id")]
        public long SubscriptionId { get; set; }

        [JsonPropertyName("updates")]
        public Dictionary<string, object> Updates { get; set; } = new();
    }
}
