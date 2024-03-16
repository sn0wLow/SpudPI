using System.Text.Json.Serialization;

namespace VoicemodAPI
{
    public class GetUserResponseActionObject
    {
        [JsonPropertyName("userId")]
        public string? UserID { get; set; }
    }
}
