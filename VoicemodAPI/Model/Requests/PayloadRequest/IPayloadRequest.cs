using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public interface IPayloadRequest<T> : IVoiceModRequest where T : class
    {
        [JsonIgnore]
        public T Payload { get; set; }
    }
}
