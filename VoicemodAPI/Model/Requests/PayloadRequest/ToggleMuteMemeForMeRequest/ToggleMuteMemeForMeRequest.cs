using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VoicemodAPI.Model
{
    internal class ToggleMuteMemeForMeRequest : IPayloadRequest<ToggleMuteMemeForMeRequestPayload>
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = RequestActionConstants.ToggleMuteMemeForMeAction;

        [JsonPropertyName("id")]
        public required string ActionID { get; set; }

        [JsonPropertyName("payload")]
        public required ToggleMuteMemeForMeRequestPayload Payload { get; set; }
    }
}
