using System.Text.Json.Serialization;

namespace VoicemodAPI.Model
{
    public class GetToggleMuteMemeForMeResponseActionObject
    {
        [JsonPropertyName("value")]
        public bool? Value { get; set; }
    }
}
