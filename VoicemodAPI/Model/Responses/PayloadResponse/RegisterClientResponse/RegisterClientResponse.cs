using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class RegisterClientResponse : IPayloadResponse<RegisterClientResponsePayload>
    {
        [JsonPropertyName("actionType")]
        public string? ActionType { get; set; }

        [JsonPropertyName("appVersion")]
        public string? AppVersion { get; set; }

        [JsonPropertyName("id")]
        public string? ActionID { get; set; }

        [JsonPropertyName("context")]
        public string? Context { get; set; }

        [JsonPropertyName("payload")]
        public RegisterClientResponsePayload? Payload { get; set; }
    }

}
