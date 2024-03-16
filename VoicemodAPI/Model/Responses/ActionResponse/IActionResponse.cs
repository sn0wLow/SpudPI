using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public interface IActionResponse<T> : IVoiceModResponse where T : class, new()
    {
        [JsonIgnore]
        public T? ActionObject { get; set; }
    }
}
