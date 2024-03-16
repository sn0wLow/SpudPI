using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class PlayMemeSoundRequest : IPayloadRequest<PlayMemeSoundRequestPayload>
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = RequestActionConstants.PlayMemeSoundAction;

        [JsonPropertyName("id")]
        public required string ActionID { get; set; }

        [JsonPropertyName("payload")]
        public required PlayMemeSoundRequestPayload Payload { get; set; }
    }
}
