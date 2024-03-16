
using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class Sound
    {
        [JsonPropertyName("id")]
        public Guid ID { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("isCustom")]
        public bool? IsCustom { get; set; }

        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }

        [JsonPropertyName("playbackMode")]
        public string? PlaybackMode { get; set; }

        [JsonPropertyName("loop")]
        public bool? Loop { get; set; }

        [JsonPropertyName("muteOtherSounds")]
        public bool? MuteOtherSounds { get; set; }

        [JsonPropertyName("muteVoice")]
        public bool? MuteVoice { get; set; }

        [JsonPropertyName("stopOtherSounds")]
        public bool? StopOtherSounds { get; set; }

        [JsonPropertyName("showProLogo")]
        public bool? ShowProLogo { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("bitmapChecksum")]
        public string? BitmapChecksum { get; set; }

        [JsonIgnore]
        public bool IsPlayingMemeSound { get; set; } = false;

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not Sound)
                return false;
            else
                return this.ID.Equals(((Sound)obj).ID);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }
    }
}
