using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navbot.RealtimeApi.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// input_audio_buffer.speech_started
    /// </summary>
    public class SpeechStarted : BaseResponse
    {
        [JsonProperty("audio_start_ms")]
        public string AudioStartMs { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }
    }
}
