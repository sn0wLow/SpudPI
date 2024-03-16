using System.Text.Json.Serialization;

namespace VoicemodAPI.Model
{
    public class GetMuteMemeForMeStatusRequest : IPayloadRequest<GetMuteMemeForMeStatusPayload>
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = RequestActionConstants.GetMuteMemeForMeStatusAction;

        [JsonPropertyName("id")]
        public required string ActionID { get; set; }

        [JsonPropertyName("payload")]
        public required GetMuteMemeForMeStatusPayload Payload { get; set; }
    }
}
