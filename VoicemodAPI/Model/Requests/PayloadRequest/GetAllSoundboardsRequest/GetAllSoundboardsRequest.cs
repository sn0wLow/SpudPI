using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetAllSoundboardsRequest : IPayloadRequest<GetAllSoundboardsRequestPayload>
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = RequestActionConstants.GetAllSoundboardsAction;

        [JsonPropertyName("id")]
        public required string ActionID { get; set; }

        [JsonPropertyName("payload")]
        public required GetAllSoundboardsRequestPayload Payload { get; set; }
    }
}
