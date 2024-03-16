using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public interface IVoiceModResponse
    {
        [JsonIgnore]
        public abstract string? ActionType { get; set; }

        [JsonIgnore]
        public abstract string? AppVersion { get; set; }

        [JsonIgnore]
        public abstract string? ActionID { get; set; }

        [JsonIgnore]
        public abstract string? Context { get; set; }
    }
}
