using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetUserResponse : IActionResponse<GetUserResponseActionObject>
    {
        [JsonPropertyName("actionType")]
        public string? ActionType { get; set; }

        [JsonPropertyName("appVersion")]
        public string? AppVersion { get; set; }

        [JsonPropertyName("actionID")]
        public string? ActionID { get; set; }

        [JsonPropertyName("context")]
        public string? Context { get; set; }

        [JsonPropertyName("actionObject")]
        public GetUserResponseActionObject? ActionObject { get; set; }
    }
}
