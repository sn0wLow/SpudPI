using System.Text.Json.Serialization;

namespace VoicemodAPI.Model
{
    public class GetMuteMemeForMeStatusResponseActionObject
    {
        [JsonPropertyName("value")]
        public bool? Value { get; set; }
    }
}
