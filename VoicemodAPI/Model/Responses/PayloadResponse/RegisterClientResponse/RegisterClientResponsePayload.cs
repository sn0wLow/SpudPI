using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class RegisterClientResponsePayload
    {
        [JsonPropertyName("status")]
        public Status? Status { get; set; }
    }
}
