using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class StopAllMemeSoundsRequest : IPayloadRequest<StopAllMemeSoundsRequestPayload>
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = RequestActionConstants.StopAllMemeSoundsAction;

        [JsonPropertyName("id")]
        public required string ActionID { get; set; }

        [JsonPropertyName("payload")]
        public required StopAllMemeSoundsRequestPayload Payload { get; set; }
    }
}
