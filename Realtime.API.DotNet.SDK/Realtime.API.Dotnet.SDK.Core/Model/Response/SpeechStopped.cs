using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// input_audio_buffer.speech_stopped
    /// </summary>
    public class SpeechStopped : BaseResponse
    {
        [JsonProperty("audio_end_ms")]
        public string AudioEndMs { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }
    }
}
