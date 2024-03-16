using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetAllSoundboardsResponseActionObject
    {
        [JsonPropertyName("soundboards")]
        public List<Soundboard>? Soundboards { get; set; }
    }
}
