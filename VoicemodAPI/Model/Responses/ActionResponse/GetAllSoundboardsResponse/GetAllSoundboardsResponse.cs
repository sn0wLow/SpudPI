using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetAllSoundboardsResponse : IActionResponse<GetAllSoundboardsResponseActionObject>
    {
        [JsonPropertyName("actionType")]
        public string? ActionType { get; set; }

        [JsonPropertyName("appVersion")]
        public string? AppVersion { get; set; }

        [JsonPropertyName("id")]
        public string? ActionID { get; set; }

        [JsonPropertyName("context")]
        public string? Context { get; set; }

        [JsonPropertyName("actionObject")]
        public GetAllSoundboardsResponseActionObject? ActionObject { get; set; }
    }
}
