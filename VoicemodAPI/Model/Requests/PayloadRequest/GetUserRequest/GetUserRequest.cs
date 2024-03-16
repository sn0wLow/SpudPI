using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetUserRequest : IPayloadRequest<GetUserRequestPayload>
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = RequestActionConstants.GetUserAction;

        [JsonPropertyName("id")]
        public required string ActionID { get; set; }

        [JsonPropertyName("payload")]
        public required GetUserRequestPayload Payload { get; set; }
    }
}
