using System.Text.Json.Serialization;

namespace VoicemodAPI.Model
{
    public class GetToggleMuteMemeForMeResponse : IActionResponse<GetToggleMuteMemeForMeResponseActionObject>
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
        public GetToggleMuteMemeForMeResponseActionObject? ActionObject { get; set; }
    }
}
