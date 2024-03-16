using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class Soundboard
    {
        [JsonPropertyName("id")]
        public Guid ID { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("isCustom")]
        public bool IsCustom { get; set; }

        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }

        [JsonPropertyName("showProLogo")]
        public bool ShowProLogo { get; set; }

        [JsonPropertyName("sounds")]
        public List<Sound>? Sounds { get; set; }
    }
}
