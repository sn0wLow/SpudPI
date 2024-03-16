using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class PlayMemeSoundRequestPayload
    {
        [JsonPropertyName("FileName")]
        public required string ID { get; set; }
        [JsonPropertyName("IsKeyDown")]
        public required bool IsKeyDown { get; set; }
    }
}
