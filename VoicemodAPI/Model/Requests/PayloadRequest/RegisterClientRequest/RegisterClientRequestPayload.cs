using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class RegisterClientRequestPayload
    {
        [JsonPropertyName("clientKey")]
        public required string ClientKey { get; set; }
    }
}
