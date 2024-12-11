using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtime.API.Dotnet.SDK.Core.Model.Response
{
    /// <summary>
    /// conversation.item.input_audio_transcription.failed
    /// </summary>
    public class TranscriptionFailed : BaseResponse
    {
        [JsonProperty("item_id")]
        public string ItemId { get; set; }
        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }
        [JsonProperty("error")]
        public TranscriptionFailedError Error { get; set; }
    }

    public class TranscriptionFailedError 
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("param")]
        public string Param { get; set; }
       
    }
}
