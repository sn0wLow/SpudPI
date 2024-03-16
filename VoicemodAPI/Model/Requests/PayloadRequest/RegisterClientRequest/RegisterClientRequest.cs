using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class RegisterClientRequest : IPayloadRequest<RegisterClientRequestPayload>
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = RequestActionConstants.RegisterClientAction;

        [JsonPropertyName("id")]
        public required string ActionID { get; set; }

        [JsonPropertyName("payload")]
        public required RegisterClientRequestPayload Payload { get; set; }
    }
}
