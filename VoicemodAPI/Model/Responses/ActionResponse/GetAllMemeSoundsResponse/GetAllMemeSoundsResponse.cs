using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetAllMemeSoundsResponse : IActionResponse<GetAllMemeSoundsResponseActionObject>
    {
        [JsonPropertyName("actionType")]
        public string? ActionType { get; set; }

        [JsonPropertyName("appVersion")]
        public string? AppVersion { get; set; }

        [JsonPropertyName("actionId")]
        public string? ActionID { get; set; }

        [JsonPropertyName("context")]
        public string? Context { get; set; }

        [JsonPropertyName("actionObject")]
        public GetAllMemeSoundsResponseActionObject? ActionObject { get; set; }
    }
}
