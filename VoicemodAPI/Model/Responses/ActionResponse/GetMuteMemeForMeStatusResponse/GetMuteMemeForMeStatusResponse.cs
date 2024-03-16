using System.Text.Json.Serialization;

namespace VoicemodAPI.Model
{
    public class GetMuteMemeForMeStatusResponse : IActionResponse<GetMuteMemeForMeStatusResponseActionObject>
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
        public GetMuteMemeForMeStatusResponseActionObject? ActionObject { get; set; }
    }
}
