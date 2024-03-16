using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetBitmapRequest : IPayloadRequest<GetBitmapRequestPayload>
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = RequestActionConstants.GetBitmapAction;

        [JsonPropertyName("id")]
        public required string ActionID { get; set; }

        [JsonPropertyName("payload")]
        public required GetBitmapRequestPayload Payload { get; set; }
    }
}
