using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class MemeSound
    {
        [JsonPropertyName("FileName")]
        public Guid ID { get; set; }

        [JsonPropertyName("Profile")]
        public string? Profile { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        [JsonPropertyName("Image")]
        public string? Image { get; set; }

        [JsonPropertyName("IsCore")]
        public bool? IsCore { get; set; }
    }
}
