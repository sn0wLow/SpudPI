using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class Status
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
