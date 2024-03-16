using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetAllMemeSoundsResponseActionObject
    {
        [JsonPropertyName("listOfMemes")]
        public List<MemeSound>? ListOfMemes { get; set; }
    }
}
