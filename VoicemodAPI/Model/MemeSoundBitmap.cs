using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class MemeSoundBitmap
    {
        [JsonPropertyName("memeId")]
        public Guid MemeID { get; set; }

        [JsonPropertyName("default")]
        public string? ImageBase64 { get; set; }

        //[JsonPropertyName("selected")]
        //public string? Selected { get; set; }

        //[JsonPropertyName("transparent")]
        //public string? Transparent { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not MemeSoundBitmap)
                return false;
            else
                return this.MemeID.Equals(((MemeSoundBitmap)obj).MemeID);
        }

        public override int GetHashCode()
        {
            return this.MemeID.GetHashCode();
        }
    }
}
