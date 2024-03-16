using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public interface IPayloadResponse<T> : IVoiceModResponse where T : class, new()
    {
        [JsonIgnore]
        public T? Payload { get; set; }
    }
}
