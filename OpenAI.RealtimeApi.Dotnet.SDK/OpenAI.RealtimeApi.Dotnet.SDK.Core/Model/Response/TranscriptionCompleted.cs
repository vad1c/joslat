using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.RealtimeApi.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// conversation.item.input_audio_transcription.completed
    /// </summary>
    public class TranscriptionCompleted : BaseResponse
    {
        [JsonProperty("item_id")]
        public string ItemId { get; set; }
        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }
        [JsonProperty("transcript")]
        public string Transcript { get; set; }
    }
}
