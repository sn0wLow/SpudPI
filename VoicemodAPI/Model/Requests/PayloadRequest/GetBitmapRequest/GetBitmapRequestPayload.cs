using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetBitmapRequestPayload
    {
        [JsonPropertyName("memeId")]
        public Guid MemeID { get; set; }

        [JsonPropertyName("voiceID")]
        public string? VoiceID { get; set; }
    }
}
