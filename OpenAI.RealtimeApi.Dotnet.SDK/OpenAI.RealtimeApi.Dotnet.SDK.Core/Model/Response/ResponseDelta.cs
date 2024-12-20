using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.RealtimeApi.Dotnet.SDK.Core.Model.Response
{
    public class ResponseDelta : BaseResponse
    {
        [JsonProperty("response_id")]
        public string ResponseId { get; set; }
        [JsonProperty("item_id")]
        public string ItemId { get; set; }
        [JsonProperty("output_index")]
        public int OutputIndex { get; set; }
        [JsonProperty("content_index")]
        public int ContentIndex { get; set; }
        [JsonProperty("delta")]
        public string Delta { get; set; }

        public ResponseDeltaType ResponseDeltaType { get; set; }


    }

    public enum ResponseDeltaType
    {
        AudioTranscriptDelta,
        AudioDelta,
        AudioDone,
        TextDelta
    }
}
