using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public interface IVoiceModRequest
    {
        [JsonIgnore]
        public abstract string Action { get; set; }

        [JsonIgnore]
        public abstract string ActionID { get; set; }
    }
}
