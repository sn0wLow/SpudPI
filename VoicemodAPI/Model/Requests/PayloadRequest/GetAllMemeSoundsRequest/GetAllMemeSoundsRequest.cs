using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetAllMemeSoundsRequest : IPayloadRequest<GetAllMemeSoundsRequestPayload>
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = RequestActionConstants.GetAllMemeSoundsAction;

        [JsonPropertyName("id")]
        public required string ActionID { get; set; }

        [JsonPropertyName("payload")]
        public required GetAllMemeSoundsRequestPayload Payload { get; set; }
    }
}
