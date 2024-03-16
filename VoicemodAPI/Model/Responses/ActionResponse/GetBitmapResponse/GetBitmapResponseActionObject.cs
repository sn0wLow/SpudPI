using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetBitmapResponseActionObject
    {
        [JsonPropertyName("result")]
        public MemeSoundBitmap? MemeSoundBitmap { get; set; }

        [JsonPropertyName("memeId")]
        public Guid MemeID { get; set; }
    }
}
